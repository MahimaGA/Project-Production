import cv2
import mediapipe as mp

mphands = mp.solutions.hands
hands = mphands.Hands(
    static_image_mode= False,
    model_complexity= 1,
    min_detection_confidence= 0.7, #70%
    min_tracking_confidence= 0.7,
    max_num_hands = 1
)

def main():
    capture = cv2.VideoCapture(0) #using primary camera=0
    draw = mp.solutions.drawing_utils #draws landmarks

    try:
        while capture.isOpened():
            ret, frame = capture.read() #return(t/f), framedata

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

                print(landmarks_list) #to check

            cv2.imshow("Frame", frame) #display frame
            if cv2.waitKey(1) & 0xFF == ord('q'): #wait 1 ms and key Q 
                break #closes window

    finally:
        capture.release() #release
        cv2.destroyAllWindows() #destroy

if __name__ == '__main__':
    main()
