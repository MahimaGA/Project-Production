import cv2
import mediapipe as mp

def main():
    capture = cv2.VideoCapture(0) #using primary camera=0

    try:
        while capture.isOpened():
            ret, frame = capture.read() #return(t/f), framedata

            if not ret:
                break
            frame=cv2.flip(frame, 1) #mirroring

            cv2.imshow("Frame", frame) #display frame
            if cv2.waitKey(1) & 0xFF == ord('q'): #wait 1 ms and key Q 
                break #closes window

    finally:
        capture.release() #release
        cv2.destroyAllWindows() #destroy

if __name__ == '__main__':
    main()
