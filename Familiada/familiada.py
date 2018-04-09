# -*- coding: cp1250 -*-
import multiprocessing
import random
import time

print 'Oto gra w familiadę. Za odpowiedzi na pytania dostajesz stosowną liczbę punktow - muszą jednak zgadzać się z kluczem, nie wszystkie uznajemy \n naciśnij enter by zaczac'
raw_input()

plik=open("familiada.txt").readlines()[1:]
questions=[]
helping=[] #wprowadzam liste pomecnicza do questions ktora bede sukcesywnie dodawal jako kolejne elementy

idx=0
for line in plik:
    line=line.upper()
    try:
        if type(int(line[0]))== int: ## Sprawszenie mozliwosci konwercji na inta
            helping[1].append(line[3:-1])

    except:
        if len(line)==1: ## Jesli jest pusta linijka wtedy ma dlugosc 1 stad jedynka. w tym wypadku ja po prostu pomijamy
            pass

        else: ## Kiedy linia zaczyna sie od stringa (ergo pytania)
            ##tworzenie listy list w ktorych bedzie kolejno w liscie o indeksie 0 pytanie a w nastepnej odpowiedzi
            ## Pierwszy krok jest dodany po to by calosc zadzialala mimo ze praktycznie nie robi on nic
            
            questions.append(helping)
            helping=[]
            helping.append([])
            helping.append([])
            helping[0].append(line[:-1])
            idx+=1
            
questions.remove([]) ##Usuniecie zbednej listy
numbers=[]

## wylosowanie pytan
def next_one():
    while len(numbers)!=5:
        choosen=int(random.uniform(0,len(questions)))
        if choosen not in numbers:
            numbers.append(choosen)
##print numbers

def get_the_points(count, with_one):
    """funkcja przyznajaca punkty"""
    three=[40, 35, 25]
    four=[40, 25,20,15]
    five=[30, 25,20,15,10]
    six=[25,22,19,15,10,7]
    if count==3:
        return three[with_one]
    if count==4:
        return four[with_one]
    if count==5:
        return five[with_one]
    if count==6:
        return six[with_one]

def are_u_good(points):
    """funkcja do okreslenia jak dobrze sobie gracz poradzil"""
    three=40
    four=40
    five=30
    six=25
    maximum=0
    for i in range(5):
        lenght=len(questions[numbers[i]][1])
        if lenght==3:
            maximum+=40
        if lenght==4:
            maximum+=40
        if lenght==5:
            maximum+=30
        if lenght==6:
            maximum+=25
    percent=(100*points)/maximum
    
    if percent<=30:
        return 'Zdobylas %s procent. Co tak slabo? Postaraj sie bardziej! :D' % percent
    if percent<=50:
        return 'Zdobylas %s procent. Nooo... juz lepiej ale nadal moze byc jeszcze lepiej! :D' % percent
    if percent<=75:
        return 'Zdobylas %s procent. Ladnie, ladnie ^^ A teraz jeszcze troszeczke bardziej sie postaraj!' % percent
    if percent<=90:
        return 'Zdobylas %s procent. No! Swietnie! Lecz to nie jest jeszcze szyczyt Twoich mozliwosci! :)' $procent
    if percent<=100:
        return 'Zdobylas %s procent. DOSKONALE! Lepiej już być nie może!' % percent
    

def play():
    """funkcja dla gracza"""
    points=0
    Start=time.time()
    for i in range(5):
        print questions[numbers[i]][0][0]
        answer=raw_input()
        answer=answer.upper()
        print '--------------------------------------------------------------------------------'
        End=time.time()
        if End-Start>=120: # Warunek w tym miejscu jest podany ze wzgledu na liczenie punktow- jesli uzytkownik za pozno wpisze odpowiedz nie zostanie ona uznana
            print '--------------------------------------------------------------------------------'
            print 'TO JUZ JEST KONIEC!'
            print 'Laczna ilosc Twoich punktow to:'
            good=are_u_good(points)
            print points
            print '--------------------------------------------------------------------------------'
            print ' ----------             ----------             ----------             ----------'
            return good
        is_it_more=points
        for correct in questions[numbers[i]][1]:
            idx=0
            if answer==correct:
                print 'DOBRZE!'
                print '... ... ...'
                points+=get_the_points(len(questions[numbers[i]][1]), idx)
            idx+=1
        if is_it_more==points:
            print 'Podano niewlasciwa odpowiedz'
        
        print 'zostalo Ci', round(120-(End-Start)), 'sekund'
    print '--------------------------------------------------------------------------------'
    print 'TO JUZ JEST KONIEC!'
    print 'Laczna ilosc Twoich punktow to:'
    good=are_u_good(points)
    print points
    print '--------------------------------------------------------------------------------'
    print '----------             ----------             ----------             ----------'
    return good
print next_one()
print play()
print 'Chcesz zagrac jeszcze raz? :3'
one_more=raw_input()
one_more=one_more.upper()
while one_more=='TAK':
    numbers=[]
    print next_one()
    print play()
    print 'Chcesz zagrac jeszcze raz? :3'
    one_more=raw_input()
    one_more=one_more.upper()
print 'Nacisnij enter zeby zakonczyc...'
raw_input()







        
    
