# TheNormalThingSourceCodes
The Normal Thing Unity Project Source Codes

## ABOUT

[![Video Label](http://img.youtube.com/vi/P8Md1caFsu0/0.jpg)](https://www.youtube.com/watch?v=P8Md1caFsu0&ab_channel=OrataStudio)

The Normal Thing is the first game I made with Unity 3d. I've worked with 3 graphic designers and 2 story designers for 7 months(03.2020~). I've programmed alone.

The player controls a puppy awakening alone in a dark and mysterious forest. You have to unravel mysteries by solving many puzzles here.

I will introduce the puzzles and systems implemented in this game through Unity 3d.

## INDEX

[1.Various Puzzles 다양한 퍼즐들](#various-puzzles)

[2.Enemy Randomly Appearing and Chasing 랜덤하게 출현하고 따라오는 적](#enemy-randomly-appearing-and-chasing)

[3.Save and Load 세이브와 로드](#save-and-load)

[4.Acquisition and Use of Items 아이템 습득과 사용](#acquisition-and-use-of-items)


## Various Puzzles

1.Rotate Puzzle 회전하는 퍼즐

<img src="https://user-images.githubusercontent.com/70127676/93859850-25d51800-fcf9-11ea-8b1a-316555aded4d.gif" width="100%" height="100%">

Implemented functions 구현한 기능들

-블록 회전 건수 : 블록 당 4 종
-모든 블록의 무작위 회전 (정답 제외)
-Number of block rotation cases: 4 types per block
-Random rotation of all blocks (except for correct answer)

2.Polyomino 폴리오미노

<img src="https://user-images.githubusercontent.com/70127676/93859916-37b6bb00-fcf9-11ea-9232-cd3fbf2ea2ac.gif" width="100%" height="100%">

Implemented functions 구현한 기능들

- Randomly connecting blocks : double list 임의로 블록들 연결 : 더블리스트

        public List<Block> linkedBlock = new List<Block>(); //특정 블록에 연결된 블록들의 리스트
        public List<int> linkedNum = new List<int>();   //특정 블록에 연결된 블록들 번호의 리스트 {0,1,2}
        public List<List<int>> lastNum = new List<List<int>>(); //특정 블록에 연결된 블록 번호들의 리스트 {0,1,2}, {3,4,8,9} ...

- Limiting where to place blocks 블록 놓는 위치 제한

3.Jigsaw Puzzle 직쏘 퍼즐

<img src="https://user-images.githubusercontent.com/70127676/93859883-2c638f80-fcf9-11ea-8388-b35f7d833470.gif" width="100%" height="100%">

- Checking the location of blocks 블록들의 위치 체크

4.Sliding Puzzle 슬라이딩 퍼즐

<img src="https://user-images.githubusercontent.com/70127676/93860447-173b3080-fcfa-11ea-9341-8224531c3690.gif" width="100%" height="100%">

-Choose between left & right and up & down 좌우/상하 이동 중 선택
-Calculate the maximum distance and move to the end 최대 이동 거리 계산 후 벽 끝으로 이동

## Enemy Randomly Appearing and Chasing

Implemented functions

- Appearance cooldown : Every 3 seconds, a random number from 0 to 99 is drawn, and if the conditions are met, enemies appear. Through this condition, the probability of appearance can be adjusted.

- Following the player or not

<img src="https://user-images.githubusercontent.com/70127676/93879846-72c7e700-fd17-11ea-9d4e-aba14f683e38.gif" width="100%" height="100%">

* 특정 위치를 지정해주면 먼저 그 위치로 이동 후 플레이어를 따라감.

- Moving the map with the player

<img src="https://user-images.githubusercontent.com/70127676/93876959-a3f1e880-fd12-11ea-916b-e3aeac22fbb0.gif" width="100%" height="100%">

- Conditions to be destroyed

<img src="https://user-images.githubusercontent.com/70127676/93876966-a6ecd900-fd12-11ea-940d-126d0b78c1c7.gif" width="100%" height="100%">

- Game over

<img src="https://user-images.githubusercontent.com/70127676/93876951-a05e6180-fd12-11ea-8457-e972bc2bec51.gif" width="100%" height="100%">

## Save and Load

## Acquisition and Use of Items
