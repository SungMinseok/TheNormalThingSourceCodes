# TheNormalThingSourceCodes
The Normal Thing Unity Project Source Codes

## ABOUT

[![Video Label](http://img.youtube.com/vi/P8Md1caFsu0/0.jpg)](https://www.youtube.com/watch?v=P8Md1caFsu0&ab_channel=OrataStudio)

The Normal Thing is the first game I made with Unity 3d. I've worked with 3 graphic designers and 2 story designers for 7 months(03.2020~). I've programmed alone.

The player controls a puppy awakening alone in a dark and mysterious forest. You have to unravel mysteries by solving many puzzles here.

I will introduce the puzzles and systems implemented in this game through Unity 3d.

## INDEX

[1.Various Puzzles //다양한 퍼즐들](#various-puzzles)

[2.Enemy Randomly Appearing and Chasing //랜덤하게 출현하고 따라오는 적](#enemy-randomly-appearing-and-chasing)

[3.Save and Load //세이브와 로드](#save-and-load)

[4.Acquisition and Use of Items //아이템 습득과 사용](#acquisition-and-use-of-items)


## Various Puzzles

1.Rotate Puzzle //회전하는 퍼즐

<img src="https://user-images.githubusercontent.com/70127676/93859850-25d51800-fcf9-11ea-8b1a-316555aded4d.gif" width="100%" height="100%">

Implemented functions //구현한 기능들

-Number of block rotation cases: 4 types per block //블록 회전 경우의 수 : 블록 당 4 가지
-Random rotation of all blocks (except for correct answer) //모든 블록의 무작위 회전 (정답 제외)

2.Polyomino //폴리오미노

<img src="https://user-images.githubusercontent.com/70127676/93859916-37b6bb00-fcf9-11ea-9232-cd3fbf2ea2ac.gif" width="100%" height="100%">

Implemented functions //구현한 기능들

- Randomly connecting blocks : double list //임의로 블록들 연결 : 더블리스트

        public List<Block> linkedBlock = new List<Block>(); //특정 블록에 연결된 블록들의 리스트
        public List<int> linkedNum = new List<int>();   //특정 블록에 연결된 블록들 번호의 리스트 {0,1,2}
        public List<List<int>> lastNum = new List<List<int>>(); //특정 블록에 연결된 블록 번호들의 리스트 {0,1,2}, {3,4,8,9} ...

- Limiting where to place blocks //블록 놓는 위치 제한

3.Jigsaw Puzzle //직쏘 퍼즐

<img src="https://user-images.githubusercontent.com/70127676/93859883-2c638f80-fcf9-11ea-8388-b35f7d833470.gif" width="100%" height="100%">

- Checking the location of blocks //블록들의 위치 체크

4.Sliding Puzzle //슬라이딩 퍼즐

<img src="https://user-images.githubusercontent.com/70127676/93860447-173b3080-fcfa-11ea-9341-8224531c3690.gif" width="100%" height="100%">

-Choose between left & right and up & down //좌우/상하 이동 중 선택
-Calculate the maximum distance and move to the end //최대 이동 거리 계산 후 벽 끝으로 이동

## Enemy Randomly Appearing and Chasing

Implemented functions //구현한 기능들

__- Appearance cooldown //출현 쿨타임__ 
* Every 3 seconds, a random number from 0 to 99 is drawn, and if the conditions are met, enemies appear. Through this condition, the probability of appearance can be adjusted. //출현 쿨타임 : 3 초마다 0 ~ 99까지 임의의 숫자가 뽑히고 조건이 충족되면 적이 등장함. 이 조건을 통해 출현 확률을 조정할 수 있음.

__- Following the player or not //선택적 추적__
* If I specify a specific location, it first moves to that location and then follows the player. //특정 위치를 지정해주면 먼저 그 위치로 이동 후 플레이어를 따라감.

<img src="https://user-images.githubusercontent.com/70127676/93879846-72c7e700-fd17-11ea-9d4e-aba14f683e38.gif" width="100%" height="100%">

__- Moving the maps with the player //플레이어 따라 맵 이동__
* When the player moves the map, the starting point where the player moved is taken over and recreated there after a certain period of time. //플레이어가 맵을 이동하면 플레이어가 이동한 스타트 포인트를 넘겨받아 일정 시간 지난 후 그 곳에서 재생성.

<img src="https://user-images.githubusercontent.com/70127676/93876959-a3f1e880-fd12-11ea-916b-e3aeac22fbb0.gif" width="100%" height="100%">

__- Conditions to be destroyed //파괴되는 조건들__
* Destroyed when the player is not touched by the enemy for a certain period of time or moving the map multiple times. //일정 시간 동안 적에게 닿지 않거나 맵을 여러번 이동시 파괴됨.
* Forcibly destroyed when moving to a specific map. //특정 맵으로 이동하면 강제로 파괴됨.

<img src="https://user-images.githubusercontent.com/70127676/93876966-a6ecd900-fd12-11ea-940d-126d0b78c1c7.gif" width="100%" height="100%">

__- Game over //게임오버__
* When the game is over, you can return to the main menu or move to the last save point. //게임 오버되면 메인메뉴로 돌아가거나 마지막 저장 지점으로 이동가능
* All data must be initialized (map information, items, story progress, etc.) //모든 데이터들을 초기화해야함.(맵 정보, 아이템, 스토리 진행상태 등)

<img src="https://user-images.githubusercontent.com/70127676/93876951-a05e6180-fd12-11ea-8457-e972bc2bec51.gif" width="100%" height="100%">

## Save and Load

## Acquisition and Use of Items
