import os
import sys
import time
import cv2
import ctypes
import mediapipe as mp
import utility as util
import pyautogui as pg

# Disable PyAutoGUI fail-safe to prevent exceptions when cursor moves to corners
pg.FAILSAFE = False

# ——— EXIT FLAG ———
FLAG_PATH = os.path.join(os.path.dirname(os.path.abspath(__file__)), "exit.flag")

# ——— MEDIAPIPE SETUP ———
mphands = mp.solutions.hands
hands = mphands.Hands(
    static_image_mode=False,
    model_complexity=1,
    min_detection_confidence=0.7,
    min_tracking_confidence=0.7,
    max_num_hands=1
)

# ——— WIN32 CONSTANTS ———
HWND_TOPMOST        = -1
GWL_EXSTYLE         = -20
WS_EX_TOPMOST       = 0x00000008
WS_EX_NOACTIVATE    = 0x08000000
WS_EX_TOOLWINDOW    = 0x00000080
WS_EX_LAYERED       = 0x00080000
WS_EX_TRANSPARENT   = 0x00000020
SWP_NOMOVE          = 0x0002
SWP_NOSIZE          = 0x0001
SWP_SHOWWINDOW      = 0x0040
SW_SHOWNOACTIVATE   = 4


def isshoot(landmarks_list, thumb_to_index):
    return (
        util.getangle(landmarks_list[5], landmarks_list[6], landmarks_list[8]) > 90
        and util.getangle(landmarks_list[9], landmarks_list[10], landmarks_list[12]) < 40
        and thumb_to_index < 75
    )


def move_mouse(index_finger_tip):
    try:
        if index_finger_tip is not None:
            x = int(index_finger_tip.x * pg.size().width)
            y = int(index_finger_tip.y * pg.size().height)
            pg.moveTo(x, y)
    except pg.FailSafeException:
        pass


def find_tip(processed):
    if processed.multi_hand_landmarks:
        return processed.multi_hand_landmarks[0].landmark[mphands.HandLandmark.INDEX_FINGER_TIP]
    return None


def detect_gestures(frame, landmarks_list, processed, shooting_state):
    if len(landmarks_list) >= 21:
        tip = find_tip(processed)
        thumb_to_index = util.getdistance([landmarks_list[4], landmarks_list[5]])

        # Aim
        if thumb_to_index > 75 and util.getangle(landmarks_list[5], landmarks_list[6], landmarks_list[8]) > 90:
            move_mouse(tip)
            shooting_state[0] = False
            cv2.putText(frame, "Aiming", (50, 50), cv2.FONT_HERSHEY_SIMPLEX, 1, (255, 0, 0), 2)

        # Shoot
        elif isshoot(landmarks_list, thumb_to_index):
            if not shooting_state[0]:
                pg.click()
                shooting_state[0] = True
                cv2.putText(frame, "Shooting", (50, 50), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2)

        else:
            shooting_state[0] = False
            cv2.putText(frame, "Idle", (50, 50), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 0, 255), 2)


def main():
    # Open camera
    capture = cv2.VideoCapture(0, cv2.CAP_DSHOW)
    capture.set(cv2.CAP_PROP_FRAME_WIDTH, 640)
    capture.set(cv2.CAP_PROP_FRAME_HEIGHT, 480)

    # Create window once
    cv2.namedWindow("Frame", cv2.WINDOW_NORMAL)

    # Warm up camera
    for _ in range(10):
        ret, _ = capture.read()
        time.sleep(0.05)

    # Configure overlay window styles: topmost, no-activate, toolwindow, layered, transparent
    hwnd = ctypes.windll.user32.FindWindowW(None, "Frame")
    if hwnd:
        ex = ctypes.windll.user32.GetWindowLongW(hwnd, GWL_EXSTYLE)
        ctypes.windll.user32.SetWindowLongW(
            hwnd,
            GWL_EXSTYLE,
            ex | WS_EX_TOPMOST | WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW | WS_EX_LAYERED | WS_EX_TRANSPARENT
        )
        # Make window visible without activating it
        ctypes.windll.user32.ShowWindow(hwnd, SW_SHOWNOACTIVATE)
        ctypes.windll.user32.SetWindowPos(
            hwnd,
            HWND_TOPMOST,
            0, 0, 0, 0,
            SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW
        )

    shooting = [False]

    try:
        while capture.isOpened():
            ret, frame = capture.read()
            if not ret or os.path.exists(FLAG_PATH):
                break

            frame = cv2.flip(frame, 1)
            frame_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            processed = hands.process(frame_rgb)

            landmarks = []
            if processed.multi_hand_landmarks:
                lm = processed.multi_hand_landmarks[0]
                mp.solutions.drawing_utils.draw_landmarks(frame, lm, mphands.HAND_CONNECTIONS)
                landmarks = [(p.x, p.y) for p in lm.landmark]

            detect_gestures(frame, landmarks, processed, shooting)
            cv2.imshow("Frame", frame)

            if cv2.waitKey(1) & 0xFF == ord('q'):
                break

    finally:
        capture.release()
        cv2.destroyAllWindows()
        if os.path.exists(FLAG_PATH):
            os.remove(FLAG_PATH)

if __name__ == "__main__":
    main()
