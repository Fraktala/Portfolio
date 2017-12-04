#!/usr/bin/env python

## Henryk Zulinski semestr 4
## Brak klass-> w na potrzeby projektu nie byl‚y potrzebne
"""
Projekt mial na celu utworzenie maszyny ktora ma wykryc czlowieka
w zaleznosci od jego doleglosci od kamery nie robic nic
lub w ostatnim stadium nakierowac na niego laser oraz powiadomic wlasciciela o "wyeliminowaniu celu"
"""

from __future__ import print_function
import numpy as np
import cv2
from video import create_capture
from common import clock, draw_str
from email.mime.multipart import MIMEMultipart
from email.mime.image import MIMEImage
from email.mime.text import MIMEText
import RPi.GPIO as GPIO
import datetime
import time
import os
import smtplib
import urllib2
from twilio.rest import TwilioRestClient

GPIO.setwarnings(False)
GPIO.setmode(GPIO.BCM)
GPIO.setup(18, GPIO.OUT)
GPIO.setup(17, GPIO.OUT)
GPIO.setup(27, GPIO.OUT)

GPIO.output(27,0)
GPIO.output(18,0)#prawo-lewo
GPIO.output(17,0)#gora-dol
time.sleep(1)

## Pozycja zerowa
for i in range(20):
    time.sleep(0.02)
    GPIO.output(17,1)
    time.sleep(0.001475)
    GPIO.output(17,0)
    time.sleep(0.02)
    
    GPIO.output(18,1)
    time.sleep(0.001475)
    GPIO.output(18,0)


def detect(img, cascade):
    # Funkcja zwracajaca tablice pikseli
    rects = cascade.detectMultiScale(img, scaleFactor=1.3, minNeighbors=4, minSize=(30, 30),
                                     flags=cv2.CASCADE_SCALE_IMAGE)
    if len(rects) == 0:
        return []
    rects[:,2:] += rects[:,:2]
    return rects

def internet_on():
    ## Sprawdzenie czy jest polaczenie internetowe
    try:
        response=urllib2.urlopen('https://www.google.pl',timeout=1)
        return True
    except urllib2.URLError as err: pass
    return False

def SendMail(ImgFileName):
    ## Funkcja wysylajaca z podanego adresu email na drugi informacje o intruzach
    now=datetime.datetime.now()
    img_data = open(ImgFileName, 'rb').read()
    msg = MIMEMultipart()
    msg['Subject'] = 'Intruder'
    data=now.strftime('%d-%m-%Y %H:%M')
    text = MIMEText('Intruder killed! At %s' % data)
    msg.attach(text)
    image = MIMEImage(img_data, name=os.path.basename(ImgFileName))
    msg.attach(image)

    s = smtplib.SMTP('smtp.mail.yahoo.com', 587)
    s.ehlo()
    s.starttls()
    s.ehlo()
    s.login('projekt_raspbery@yahoo.com', 'Rasbian999')
    s.sendmail('projekt_raspbery@yahoo.com', 'henryk.zulinski@smcebi.edu.pl', msg.as_string())
    s.quit()

def SMS():
    ## Funkcja wysylajaca wiadomosc tekstowa na zadany numer- juz nie aktywna
    accountSid = 'ACc76646d28512528d7642775fb04a2531'
    authToken = '390158f148f3f417de97f47ca899930a'
    twilioClient = TwilioRestClient(accountSid, authToken)
    myTwilioNumber = '+48732484017'
    destCellPhone = '+48660391391'
    myMessage = twilioClient.messages.create(body = "SMS zostal wyslany", from_=myTwilioNumber, to=destCellPhone)

flag=[0,0,0]
def draw_rects(img, rects, color):
    # Funkcja majaca okreslic odleglosc intruza od urzadzenia
    
    for x1, y1, x2, y2 in rects:
        z=x2-x1
        # print (z)
        if z<45:
            #print ('Daleko')
            if flag[0]==0:
                flag[1], flag[2] = 0,0
                flag[0]=1
                GPIO.output(27,0)
                
        elif z>=45 and z<75:
            #print ('Srednio')
            if flag[1]==0:
                flag[0], flag[2] = 0,0
                flag[1]=1
                os.system('omxplayer What.mp3')
                
            GPIO.output(27,1)
            xo=(((x2-x1)/2.0)+x1)/512.0
            yo=y2/512.0
            xk,yk=xo*0.00035,yo*0.00035
                
            for i in range(5):
                time.sleep(0.02)
                #gora-dol
                GPIO.output(17,1)
                time.sleep(0.00135+(yk))
                GPIO.output(17,0)
                    
                time.sleep(0.02)
                #prawo-lewo
                GPIO.output(18,1)
                time.sleep(0.00165-xk)
                GPIO.output(18,0)
        else:
            #print ('blisko')
            if flag[2]==0:
                flag[0], flag[1] = 0,0
                flag[2]=1
                os.system('omxplayer ISaw.mp3')
                GPIO.output(27,0)
                cv2.imwrite('Intruder.jpg', img)
                internet= internet_on()
                if internet== True:
                    SendMail('Intruder.jpg')
                    #SMS() ##Funkcja zostaa przetestowana- zadziala lecz w zwizku z palatnoscia uslugi konto zostalo usuniete i nie bedzie prezentowane

        cv2.rectangle(img, (x1, y1), (x2, y2), color, 2)
        

if __name__ == '__main__':
    import sys, getopt
    print(__doc__)

    ## Sprawdzenie czy kamera jest podlaczona
    args, video_src = getopt.getopt(sys.argv[1:], '', ['cascade=', 'nested-cascade='])
    try:
        video_src = video_src[0]
    except:
        video_src = 0
    args = dict(args)
    cascade_fn = args.get('--cascade', "../../data/haarcascades/haarcascade_frontalface_alt.xml")
    nested_fn  = args.get('--nested-cascade', "../../data/haarcascades/haarcascade_eye.xml")

    cascade = cv2.CascadeClassifier(cascade_fn)
    nested = cv2.CascadeClassifier(nested_fn)

    cam = create_capture(video_src, fallback='synth:bg=../data/lena.jpg:noise=0.05')

    while cv2.waitKey(33)!=ord('a'):
        # a- przycisk przerwania wykonywania programu
        ret, img = cam.read()
        gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
        gray = cv2.equalizeHist(gray)

        t = clock()
        rects = detect(gray, cascade)
        vis = img.copy()
        draw_rects(vis, rects, (0, 255, 0))
        if not nested.empty():
            for x1, y1, x2, y2 in rects:
                roi = gray[y1:y2, x1:x2]
                vis_roi = vis[y1:y2, x1:x2]
                subrects = detect(roi.copy(), nested)
        dt = clock() - t

        draw_str(vis, (20, 20), 'time: %.1f ms' % (dt*1000))
        cv2.imshow('facedetect', vis)

    # Zakonczenie programu    
    GPIO.output(27,0)
    cv2.destroyAllWindows()
