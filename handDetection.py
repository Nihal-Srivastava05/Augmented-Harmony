import cv2
from cvzone.HandTrackingModule import HandDetector
import socket

width, height = 1280, 720

cap = cv2.VideoCapture(0)
cap.set(3, width)
cap.set(4, height)

detector = HandDetector(maxHands=1, detectionCon=0.8)

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddressPort = ("127.0.0.1", 5052)
# serverAddressPort = ("192.168.206.216", 5052)
# sock.bind(serverAddressPort)

while True:
	success, img = cap.read()
	hands, img = detector.findHands(img)

	# Landmark = (x,y,z) * 21 (21 landmarks)
	data = []
	if hands:
		# Get first hand deteted
		hand = hands[0]
		lmList = hand['lmList']
		for lm in lmList:
			data.extend([lm[0], height-lm[1], lm[2]])
		sock.sendto(str.encode(str(data)), serverAddressPort)

	img = cv2.resize(img, (0, 0), None, 0.5, 0.5)
	cv2.imshow("Image", img)
	if cv2.waitKey(1) & 0xFF == ord('q'):
		break

sock.close()
cap.release()
cv2.destroyAllWindows()