# "The Normal Thing" Source Codes
 Source Codes of The Normal Thing Unity 3d Project
 
 
## PLAY (FREE)

(https://store.steampowered.com/app/1397980/THE_NORMAL_THING/?l=koreana)

## ABOUT

[![Video Label](http://img.youtube.com/vi/P8Md1caFsu0/0.jpg)](https://www.youtube.com/watch?v=P8Md1caFsu0&ab_channel=OrataStudio)

The Normal Thing is the first game I made with Unity 3d. I've worked with 3 graphic designers and 2 story designers for 7 months(03.2020~). I've programmed alone.

The player controls a puppy awakening alone in a dark and mysterious forest. You have to unravel mysteries by solving many puzzles here.

I will introduce the puzzles and systems implemented in this game through Unity 3d.

***

## INDEX

[1.Various Puzzles //다양한 퍼즐들](#various-puzzles)

[2.Enemy Randomly Appearing and Chasing //랜덤하게 출현하고 따라오는 적](#enemy-randomly-appearing-and-chasing)

[3.Save and Load //세이브와 로드](#save-and-load)

[4.Acquisition and Use of Items //아이템 습득과 사용](#acquisition-and-use-of-items)

***

## Various Puzzles

1.Rotate Puzzle //회전하는 퍼즐

<img src="https://user-images.githubusercontent.com/70127676/93859850-25d51800-fcf9-11ea-8b1a-316555aded4d.gif" width="100%" height="100%">

Implemented functions //구현한 기능들

- Number of block rotation cases: 4 types per block //블록 회전 경우의 수 : 블록 당 4 가지
- Random rotation of all blocks (except for correct answer) //모든 블록의 무작위 회전 (정답 제외)

***

2.Polyomino //폴리오미노

<img src="https://user-images.githubusercontent.com/70127676/93859916-37b6bb00-fcf9-11ea-9232-cd3fbf2ea2ac.gif" width="100%" height="100%">

Implemented functions //구현한 기능들

- Randomly connecting blocks : double list //임의로 블록들 연결 : 더블리스트

        public List<Block> linkedBlock = new List<Block>(); //특정 블록에 연결된 블록들의 리스트
        public List<int> linkedNum = new List<int>();   //특정 블록에 연결된 블록들 번호의 리스트 {0,1,2}
        public List<List<int>> lastNum = new List<List<int>>(); //특정 블록에 연결된 블록 번호들의 리스트 {0,1,2}, {3,4,8,9} ...

- Limiting where to place blocks //블록 놓는 위치 제한

***

3.Jigsaw Puzzle //직쏘 퍼즐

<img src="https://user-images.githubusercontent.com/70127676/93859883-2c638f80-fcf9-11ea-8388-b35f7d833470.gif" width="100%" height="100%">

- Checking the location of blocks //블록들의 위치 체크

***

4.Sliding Puzzle //슬라이딩 퍼즐

<img src="https://user-images.githubusercontent.com/70127676/93860447-173b3080-fcfa-11ea-9341-8224531c3690.gif" width="100%" height="100%">

- Choose between left & right and up & down //좌우/상하 이동 중 선택
- Calculate the maximum distance and move to the end //최대 이동 거리 계산 후 벽 끝으로 이동

***

## Enemy Randomly Appearing and Chasing

Implemented functions //구현한 기능들

__- Appearance cooldown //출현 쿨타임__ 
* Every 3 seconds, a random number from 0 to 99 is drawn, and if the conditions are met, enemies appear. Through this condition, the probability of appearance can be adjusted. //출현 쿨타임 : 3 초마다 0 ~ 99까지 임의의 숫자가 뽑히고 조건이 충족되면 적이 등장함. 이 조건을 통해 출현 확률을 조정할 수 있음.

***

__- Following the player or not //선택적 추적__
* If I specify a specific location, it first moves to that location and then follows the player. //특정 위치를 지정해주면 먼저 그 위치로 이동 후 플레이어를 따라감.

<img src="https://user-images.githubusercontent.com/70127676/93879846-72c7e700-fd17-11ea-9d4e-aba14f683e38.gif" width="100%" height="100%">

***

__- Moving the maps with the player //플레이어 따라 맵 이동__
* When the player moves the map, the starting point where the player moved is taken over and recreated there after a certain period of time. //플레이어가 맵을 이동하면 플레이어가 이동한 스타트 포인트를 넘겨받아 일정 시간 지난 후 그 곳에서 재생성.

<img src="https://user-images.githubusercontent.com/70127676/93876959-a3f1e880-fd12-11ea-916b-e3aeac22fbb0.gif" width="100%" height="100%">

***

__- Conditions to be destroyed //파괴되는 조건들__
* Destroyed when the player is not touched by the enemy for a certain period of time or moving the map multiple times. //일정 시간 동안 적에게 닿지 않거나 맵을 여러번 이동시 파괴됨.
* Forcibly destroyed when moving to a specific map. //특정 맵으로 이동하면 강제로 파괴됨.

<img src="https://user-images.githubusercontent.com/70127676/93876966-a6ecd900-fd12-11ea-940d-126d0b78c1c7.gif" width="100%" height="100%">

***

__- Game over //게임오버__
* When the game is over, you can return to the main menu or move to the last save point. //게임 오버되면 메인메뉴로 돌아가거나 마지막 저장 지점으로 이동가능
* All data must be initialized (map information, items, story progress, etc.) //모든 데이터들을 초기화해야함.(맵 정보, 아이템, 스토리 진행상태 등)

<img src="https://user-images.githubusercontent.com/70127676/93876951-a05e6180-fd12-11ea-8457-e972bc2bec51.gif" width="100%" height="100%">

***

## Save and Load

Implemented functions //구현한 기능들

__- Database management //데이터 베이스 관리__    

- Declare Data class to store various information //각종 정보를 저장할 Data 클래스 선언

```C#
        [System.Serializable]   //직렬화.
        public class Data{
        
                //Player`s position
                public float playerX;
                public float playerY;
                public float playerZ;
                
                //Player's state
                public List<int> playerItemInventory;
                public string currentMapName;

                //Map
                public List<int> doorEnabledList;   //등록 되어있으면 열림
                public List<int> doorLockedList;  //등록 되어있으면 잠김

                //UI
                public bool bookActivated;
                public int where;           //map에 포인트 위치

                //trigger
                public List<int> trigOverList;    //등록 되어있으면 다시 실행 안됨

                ...
        }
```

- When saving in the game, call the CallSave function to move the current information to data and then save it as a dat file in AppData in the computer user folder. //게임 내에서 저장시 CallSave 함수를 호출해서 현재 정보들을 data에 옮긴 후 컴퓨터 사용자 폴더 내 AppData에 dat파일로 저장.

```C#
        public void CallSave(int num){

                data.playerX = thePlayer.transform.position.x;
                data.playerY = thePlayer.transform.position.y;
                data.playerZ = thePlayer.transform.position.z;

                data.currentMapName = thePlayer.currentMapName;
                data.mazeNum = thePlayer.mazeNum;

                data.doorEnabledList = theDB.doorEnabledList;
                data.doorLockedList = theDB.doorLockedList;

                data.bookActivated = theDB.bookActivated;

                data.trigOverList = theDB.trigOverList;

                ...

                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + "/" + Application.version + "/SaveFile" + num +".dat");
                bf.Serialize(file, data);
                file.Close();
        }
```

- When loading in the game, the CallLoad function is called to load the dat file into AppData in the computer user folder and transfer the data information. //게임 내에서 불러오기시 CallLoad 함수를 호출해서 컴퓨터 사용자 폴더 내 AppData에 dat파일을 불러와서 data의 정보들을 옮겨 받음.

```C#
        public void CallLoad(int num){

                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + Application.version + "/SaveFile" + num +".dat", FileMode.Open);

                if(file != null && file.Length >0){

                    data =(Data)bf.Deserialize(file);

                    thePlayer.currentMapName = data.currentMapName;

                    vector.Set(data.playerX, data.playerY, data.playerZ);
                    thePlayer.transform.position = vector;

                    theDB.doorEnabledList =data.doorEnabledList;
                    theDB.doorLockedList =data.doorLockedList;

                    theDB.bookActivated = data.bookActivated;

                    theDB.trigOverList = data.trigOverList;

                    ...
                    
                }

                file.Close();
        }    
```

__- Data visualization //데이터 시각화__   

- When saving/loading, it shows the save time and the last location in the game. //저장/불러오기 시 저장시간과 게임 내 마지막 위치를 보여줌.

<img src="https://user-images.githubusercontent.com/70127676/93990983-bd546c80-fdc6-11ea-8b18-8019ccf29361.gif" width="100%" height="100%">

<img src="https://user-images.githubusercontent.com/70127676/93990991-bf1e3000-fdc6-11ea-86aa-7933bb51c7d6.gif" width="100%" height="100%">

## Acquisition and Use of Items

Implemented functions //구현한 기능들

__- Item acquisition //아이템 습득__

<img src="https://user-images.githubusercontent.com/70127676/93990996-c0e7f380-fdc6-11ea-95a3-90b9f4c3bacd.gif" width="100%" height="100%">

__- Click of item and detailed description display //아이템 개별 클릭과 상세설명 표시__

<img src="https://user-images.githubusercontent.com/70127676/93991001-c2b1b700-fdc6-11ea-836b-aae769c802d4.gif" width="100%" height="100%">

__- Specific actions available when using items //아이템 사용시 특정 액션 가능__

<img src="https://user-images.githubusercontent.com/70127676/93991011-c5141100-fdc6-11ea-83da-ac830ff7f637.gif" width="100%" height="100%">

<img src="https://user-images.githubusercontent.com/70127676/93991021-c80f0180-fdc6-11ea-9e23-214d7fe927bf.gif" width="100%" height="100%">
