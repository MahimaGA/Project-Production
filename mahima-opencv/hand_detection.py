import cv2
import mediapipe as mp
import utility as util
import pyautogui as pg
import socket
import time

#setting up mediapipe
mphands = mp.solutions.hands
hands = mphands.Hands(
    static_image_mode= False,
    model_complexity= 1,
    min_detection_confidence= 0.7, #70%
    min_tracking_confidence= 0.7,
    max_num_hands = 1
)


#setting up TCP client 
HOST, PORT = "127.0.0.1", 65432
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

for attempt in range(10):
    try:
        sock.connect((HOST, PORT))
        print("Connected to Godot TCP server.")
        break
    except ConnectionRefusedError:
        print(f"Attempt {attempt+1}/10: server not up yet, retrying in 0.5s...")
        time.sleep(0.5)
else:
    raise RuntimeError("Could not connect to Godot server on port 65432")

prev_px, prev_py = None, None


def send_motion(dx, dy):
    """Send delta‐motion as 'dx,dy' over TCP."""
    msg = f"{dx},{dy}".encode("utf-8")
    sock.sendall(msg)


def isshoot(landmarks_list, thumb_to_index):
    return (util.getangle(landmarks_list[5], landmarks_list[6], landmarks_list[8]) > 90 and 
            util.getangle(landmarks_list[9], landmarks_list[10], landmarks_list[12]) < 50 and
            thumb_to_index < 75 )

def move_mouse(index_finger_tip):
    if index_finger_tip is not None:
        x = int(index_finger_tip.x * pg.size().width) #x coordinate and screen width
        y= int(index_finger_tip.y * pg.size().height) #y coordinate and screen height
        pg.moveTo(x, y) #move to x,y on screen

def detect_gestures(frame, landmarks_list, processed, shooting_state):
    global prev_px, prev_py

    if len(landmarks_list) >= 21:
        
        index_finger_tip = find_tip(processed)
        thumb_to_index = util.getdistance([landmarks_list[4], landmarks_list[5]])
        
        #aim (curosr movement)
        if thumb_to_index > 75 and util.getangle(landmarks_list[5], landmarks_list[6], landmarks_list[8]) > 90:
            # convert to screen pixels
            px = int(index_finger_tip.x * pg.size().width)
            py = int(index_finger_tip.y * pg.size().height)

            if prev_px is not None and prev_py is not None:
                dx, dy = px - prev_px, py - prev_py
                send_motion(dx, dy)

            prev_px, prev_py = px, py
            
            #move_mouse(index_finger_tip)
            shooting_state[0] = False
            cv2.putText(frame, "Aiming", (50, 50), cv2.FONT_HERSHEY_SIMPLEX, 1, (255, 0, 0), 2)

        #shoot (enter key)
        elif isshoot(landmarks_list, thumb_to_index):
            if not shooting_state[0]:  # only shoot on first detection
                pg.press('enter')
                shooting_state[0] = True            
                cv2.putText(frame, "Shooting", (50, 50), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2)

        else:
            shooting_state[0] = False
            cv2.putText(frame, "Idle", (50, 50), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 0, 255), 2)

def find_tip(processed):
    if processed.multi_hand_landmarks:
        hand_landmarks = processed.multi_hand_landmarks[0]
        return hand_landmarks.landmark[mphands.HandLandmark.INDEX_FINGER_TIP]
    
    return None

def main():
    capture = cv2.VideoCapture(0) #using primary camera=0
    draw = mp.solutions.drawing_utils #draws landmarks

    #resolution size
    capture.set(cv2.CAP_PROP_FRAME_WIDTH, 640)
    capture.set(cv2.CAP_PROP_FRAME_HEIGHT, 480)

    shooting = [False] #list form so that we can change inside a function

    try:
        while capture.isOpened():
            ret, frame = capture.read() #return(true/false), framedata

            if not ret:
                break
            frame=cv2.flip(frame, 1) #mirroring
            frameRGB= cv2.cvtColor(frame,cv2.COLOR_BGR2RGB) #bluegreenred to RGB
            
            processed = hands.process(frameRGB)

            landmarks_list = []

            if processed.multi_hand_landmarks:
                hand_landmarks = processed.multi_hand_landmarks[0]
                draw.draw_landmarks(frame, hand_landmarks, mphands.HAND_CONNECTIONS) #draw handlandmarks, connections

                for lm in hand_landmarks.landmark: #taking each and appending
                    landmarks_list.append((lm.x, lm.y)) #add coordinates to list

            detect_gestures(frame, landmarks_list, processed, shooting)

                #print(landmarks_list) #to check

            cv2.imshow("Frame", frame) #display frame
            if cv2.waitKey(1) & 0xFF == ord('q'): #wait 1 ms and key Q 
                break #closes window

    finally:
        capture.release() #release
        cv2.destroyAllWindows() #destroy

if __name__ == '__main__':
    main()
