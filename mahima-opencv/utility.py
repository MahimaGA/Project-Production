import numpy as np

def getangle(A,B,C):
    radians = np.arctan2( C[1] - B[1], C[0] - B[0]) - np.arctan2(A[1] - B[1], A[0] - B[0])
    angle = np.abs(np.degrees(radians))
    return angle

def getdistance(landmark_list):
    if len(landmark_list)<2:
        return None
    (x1,y1), (x2,y2) = landmark_list[0], landmark_list[1]
    L= np.hypot(x2-x1, y2-y1)
    return np.interp(L, [0,1], [0,1000])