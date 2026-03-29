import mediapipe as mp
from mediapipe.tasks import python
from mediapipe.tasks.python import vision
import cv2
import socket

model_path = 'hand_landmarker.task'

BaseOptions = mp.tasks.BaseOptions
HandLandmarker = mp.tasks.vision.HandLandmarker
HandLandmarkerOptions = mp.tasks.vision.HandLandmarkerOptions
VisionRunningMode = mp.tasks.vision.RunningMode

# UDP setup
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
server_address = ("127.0.0.1", 5052)

cap = cv2.VideoCapture(0)

options = HandLandmarkerOptions(
    base_options=BaseOptions(model_asset_path=model_path),
    running_mode=VisionRunningMode.VIDEO
)

with HandLandmarker.create_from_options(options) as landmarker:
    frame_count = 0
    while True:
        ret, frame = cap.read()
        if not ret:
            break

        rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data=rgb_frame)
        result = landmarker.detect_for_video(mp_image, frame_count)
        frame_count += 33  # approx 30fps

        gesture = "NONE"

        if result.hand_landmarks:
            for i, hand_landmarks in enumerate(result.hand_landmarks):
                handedness = result.handedness[i][0].category_name  # 'Left' or 'Right'
                landmarks = hand_landmarks
                tips = [4, 8, 12, 16, 20]
                fingers = []

                # Thumb
                if handedness == 'Right':
                    if landmarks[4].x > landmarks[3].x:
                        fingers.append(1)
                    else:
                        fingers.append(0)
                else:  # Left hand
                    if landmarks[4].x < landmarks[3].x:
                        fingers.append(1)
                    else:
                        fingers.append(0)

                # Other fingers
                for tip in tips[1:]:
                    if landmarks[tip].y < landmarks[tip-2].y:
                        fingers.append(1)
                    else:
                        fingers.append(0)

                total_fingers = sum(fingers)

                if total_fingers == 0:
                    gesture = "FIST"
                elif total_fingers == 5:
                    gesture = "OPEN"

        # send gesture to Unity
        sock.sendto(gesture.encode(), server_address)

        cv2.putText(frame, gesture, (50,100),
                    cv2.FONT_HERSHEY_SIMPLEX,1,(0,255,0),2)

        cv2.imshow("Hand Gesture", frame)

        if cv2.waitKey(1) & 0xFF == 27:
            break

cap.release()
cv2.destroyAllWindows()