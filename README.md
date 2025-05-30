# Ordo Vox Mortis

## ▪️게임 소개

- 프로젝트명 : OVM
    - 게임 제목: **Ordo Vox Mortis**
- 플랫폼: PC
- 게임 장르 : 퍼즐/액션/암살 3D 리듬게임
- 개발 환경 : Unity
- 제작 기간 : 2025.04.11 ~ (진행중)
- 게임 플레이 컨셉:
    - 암살을 설계하고 음악의 박자에 맞춰 실행하는 암살 시뮬레이션 게임
    - 각 스테이지는 타겟의 스토리에 맞춰 다르게 구성됨
    - 플레이어는 ‘암살 수행자’로, 탐색과 시뮬레이션 모드를 통해 암살 루트를 완성
- 시놉시스
    
    > 암살 조직의 후계자인 소년은,
    감각이 남달라 단 몇 초 만에 암살 계획을 완성한다.
    
    과거에 피에 대한 트라우마가 있는 탓에
    조용한 암살에 특화되어 있다.
    직접 손에 피를 묻히지 않고, 도구와 환경을 이용해
    타겟을 ‘설계된 죽음’으로 이끈다.
    
    그러던 어느 날, 익숙한 흔적의 초대장를 마주한 소년은, 
    처음으로 설계에 흔들리기 시작한다.
    > 

## ▪️**팀원 소개**

| **이름** | **담당 역할** |
| --- | --- |
| **김효재** | 기획 |
| **박소희** | 매니저/NPC/블럭/저장 |
| **김태환** | 리듬 |
| **최시훈** | UI/사운드 |
| **최종훈** | 플레이어/카메라/NPC |

---

## ▪️**조작키**

| **구분** | **키** |
| --- | --- |
| 이동 | WASD |
| 상호작용 | E |
| 뛰기 | Shift |
| 점프 | Spacebar |
| 앉기 | V |
| 시뮬레이션 모드 토글 | Tab |
| 리듬 타격 | Spacebar |
| 닫기/취소 | ESC |
| 설정 | M |

---

## ▪️프로젝트 화면
<details>
<summary> 프로젝트 화면 </summary>
<div markdown="1">

    **게임 시작 화면**
    
   ![start](https://github.com/user-attachments/assets/a719e150-78ac-4d77-a2f1-bedcc2b88cba)

    **의뢰 수락 화면**
    
   ![dmlfhl](https://github.com/user-attachments/assets/ee688f87-f507-4bd7-9f01-0bef84200bdf)

    **음악 선택 화면**
    
   ![music](https://github.com/user-attachments/assets/e0e13ff7-ad6a-46eb-8f41-69dc54972de0)

    **시뮬레이션 모드 및 암살 설계 화면**
    
   ![simul](https://github.com/user-attachments/assets/a14d8aef-e004-4e48-9829-80ff83461d5d)

    **암살 수행(리듬게임 - 3D)**

   ![3D](https://github.com/user-attachments/assets/a2a9a2b7-c83e-4c2a-97b7-ee3553dcfdfe)
    
    **암살 수행(리듬게임 2D)**
    
   ![2D](https://github.com/user-attachments/assets/3f6325dd-86e5-4c9a-b659-4d3a5a9d8d8f)

</div>
</details>

---

## ▪️주요 기능
<details>
<summary>FSM</summary>
<div markdown="1">
    추상화된 클래스를 기반으로 인터페이스륻 도입하고 플레이어와 타겟(NPC)의 상태 전환 로직을 분리했습니다. 이를 바탕으로 각각의 상태를 클래스로 구현함으로써 단일 책임 원칙을 준수하고 각 상태의 동작이 작동되도록 설계하였습니다.
    
![image](https://github.com/user-attachments/assets/1f6c7816-460b-42bb-a4eb-91eab1a05c1d)
   
1. 새로운 상태를 추가하거나 기존 동작을 수정할 때 관련 클래스만 변경하면 되어서 전체 코드 안정성을 유지하면서 빠르게 확장이 가능합니다.
2. 상태별 로직이 명확하게 분리되어 있어 가독성이 좋습니다.
3. 객체지향 설계 원칙(단일 책임, 개방/폐쇄 원칙) 준수로 코드 품질 및 테스트 용이성 확보가 됩니다.
   
</div>
</details>
    
<details>
<summary> 스크롤 텍스트 창 UI</summary>
<div markdown="1">
    유니티의 `RectMask2D` 기능을 사용하여, 해당 컴포넌트를 가지고 있는 창을 하나 만든 뒤,
     코루틴으로 해당 창내에서, 해당 TMPro 텍스트가 세팅한 값 대로, 종료 지점에 도착하게 되면 
    다시 우측에서 좌측으로 흐르는 효과를 완성 시킬 수 있었습니다
    
    - `RectMask2D`로 UI창 위에 겹쳐 보이는 문제를 해결 할 수 있었습니다

</div>
</details>

<details>
<summary> 세이브 로드</summary>
<div markdown="1">

    
세이브-로드 시스템은 저장 전용 클래스를 기반으로 직렬화된 JSON 파일에 게임 상태를 기록하고, 각 상황에 맞는 데이터만을 선택적으로 불러와 복원합니다. 이벤트 해금 정보는 별도 파일로 분리되어 영구 유지되며, 일반/히든/이벤트 세이브를 유연하게 관리할 수 있도록 구성되어 있습니다.
    
- 저장
  
  - 현재 스테이지의 정보(블록/이벤트 배치, 선택된 음악 등) 상태를 저장 구조체 `SaveData` 에 기록
    
- 로드
  
  - 세이브 슬롯 상호 작용 시 해당하는 일반/히든/이벤트 저장 파일이 있는 지 확인 후 슬롯을 띄우고, 리플레이 시 저장 파일을 통해 스테이지를 재배치하여 상태 복원

</div>
</details>
    
<details>
<summary> 리듬 박자 계산 </summary>
<div markdown="1">

기본 시간 계산 : 60 / bpm
    
추가 리스트로 박자를 입력 받아서 박자간의 시간 계산을 함
    
1박 : 1 / 반박 : 2 / 두박 : 0.5
    
(기본 시간) / (박자) 
    
(왼쪽 사진) bpm과 박자 리스트를 입력하면 (오른쪽 사진) 실제로 노트가 입력 받는 시간이 계산 됨
    
  
  ![image 1](https://github.com/user-attachments/assets/2971ca9f-b37d-4f95-b28c-9bbe8312fb6d)

  ![image 2](https://github.com/user-attachments/assets/a57fa5dc-44f0-480a-be77-a0034947b8bf)

    
</div>
</details>

---

## ▪️코드 설명 및 주석

<details>
<summary> RhythmManager </summary>
<div markdown="1">

RhythmManager는 위의 두 리듬게임을 관리를 해준다

<details>
<summary>`IRythmAction`</summary>
<div markdown="1">

![image 3](https://github.com/user-attachments/assets/5cd6aad6-9633-4ad8-afda-ba941b2a7e21)

 GhostManager와 QTEManager를 한번에 처리할 수 있도록 IRythmAction 인터페이스를 구현하도록 만들었다.

</div>
</details>

<details>
<summary>  `AnimationCurve` 변수의 역할 </summary>
<div markdown="1">

![image 4](https://github.com/user-attachments/assets/83d7b738-7c32-41c3-a31d-4845c1fa7f7c)

![image 5](https://github.com/user-attachments/assets/7298a127-e41c-48cc-b94a-979c54b78521)

![image 6](https://github.com/user-attachments/assets/fd9bf36e-9aa4-421d-9de9-e1c89897ab86)

커브값을 입력 받아서 적절한 타이밍에 다른 효과음을 출력할 수 있게 만들었다. (0≤x≤1, 0≤y≤1)
        
이 값은 GhostManager에서 값을 읽어와서 수치에 맞게 특정 효과음을 출력할 수 있게 한다.
                
</div>
</details>

<details>
<summary> 리듬 게임 시작</summary>
<div markdown="1">

![image 7](https://github.com/user-attachments/assets/c03f32e4-6391-4f80-bbc1-b3e5d3c268b3)

처음 시작할 땐 isPlaying을 true로 초기값을 세팅해서 리듬게임을 시작하지 않게 만든다.
        
IRhythmAction 리스트에 실행할 리듬액션을 넣어준 뒤 isPlaying을 false로 바꿔주면 차례대로 재생한다.
        
![image 8](https://github.com/user-attachments/assets/1d85e43b-8780-422e-ba49-b3ad3b45c352)

음악을 재생하면 음악이 재생되는 시간을 저장을 하고 리듬게임의 총 시간을 계산한다.
        
![image 9](https://github.com/user-attachments/assets/5fb109c7-43ff-44d4-a83c-a0054ceeb570)

특정 조건이 있는 경우 그만큼 딜레이를 주고 시작을 한다. (ex. 노래 시작 전에 공백이 있는 경우…)
        
없을 경우 바로 시작하고 리듬게임이 하나가 끝나면 곧바로 다음 리듬게임을 실행한다.
        
 ![image 10](https://github.com/user-attachments/assets/7cf1adbc-a39b-4e66-b941-431e663c5895)

순서에 맞는 리듬게임과 그에 맞게 설정된 타임라인 카메라를 실행한다.

</div>
</details>    

</div>
</details>

    
<details>
<summary> 2D 리듬 게임 구현(QTE) </summary>
<div markdown="1">
 [리듬 박자 계산]
    
![image 11](https://github.com/user-attachments/assets/e45c327a-a507-4907-92a8-eca448984930)

<details>
<summary> QTE 리듬 게임 구조</summary>
<div markdown="1">

단노트와 롱노트는 QTE 클래스를 상속받는다.
        
![image 12](https://github.com/user-attachments/assets/c9e0b702-39e5-47cc-9540-ff3cbcee5930)

</div>
</details>

<details>
<summary>단 노트</summary>
<div markdown="1">

![image 13](https://github.com/user-attachments/assets/4f9546ad-bd8d-4e99-b74a-38dc76068599)

외곽선의 scale이 1초동안 2→1로 줄어든다.
        
정해진 판정이내에 처리를 하지않으면 알아서 실패처리를 한다.
        
![image 14](https://github.com/user-attachments/assets/f510e541-40a7-4f9d-b680-3449737c3066)

판정에 따라 적절한 처리를 해주고 사라진다.

</div>
</details>


<details>
<summary>롱노트</summary>
<div markdown="1">

![image 15](https://github.com/user-attachments/assets/f625c7dc-50e0-4e39-b9bc-eaf13c417d43)

외곽선의 경우는 단노트와 비슷하고 추가로 누를 시간을 추가로 처리를 해줄 멤버변수 및 함수 추가해줬다.
        

</div>
</details>


<details>
<summary>QTE Manager</summary>
<div markdown="1">

![image 16](https://github.com/user-attachments/assets/8fdc994a-c2b3-4e38-915b-dc2f1de77d6e)
      
QTEManager는 QTE들을 생성하고 관리를 해준다.
        
![image 17](https://github.com/user-attachments/assets/e5f2fae7-3aab-4a5d-a1c8-f13edd887ec4)
   
시작시 화면 크기를 받아서 QTE가 생성될 수 있는 위치를 알아서 계산해준다.
        
![image 18](https://github.com/user-attachments/assets/dcaa30b0-8146-491a-97d1-b6a723a08423)
    
 특정 시간마다 QTE를 생성하기 위해서 코루틴을 이용했다.
        
 ```csharp

 IEnumerator MakeQTE()
 {
            QTE qte;
            UI_QTE qteUI = UIManager.Instance.ShowUI<UI_QTE>("QTE_UI");
            qteUI.transform.SetAsFirstSibling();
            RhythmManager.Instance.checkJudgeText.transform.SetAsLastSibling();
        
            if (delayTime < 0) //delayTime이 설정 되어있는 경우 그만큼 딜레이 주고 재생
            {
                PlayQTEMusic();
            }
            else
            {
                Invoke("PlayQTEMusic", delayTime);
            }
        
            isAllNoteEnd = false;
        
            if (pointNoteList.Count < beats.Count)
            {
                pointNoteList = new List<bool>();
                for (int i = 0; i < beats.Count; i++)
                    pointNoteList.Add(false);
            }
        
            if (isLongNote.Count < beats.Count)
            {
                isLongNote = new List<bool>();
                for (int i = 0; i < beats.Count; i++)
                    isLongNote.Add(false);
            }
        
            if (qtePosition.Count < beats.Count)
            {
                qtePosition = new List<int>();
                for (int i = 0; i < beats.Count; i++)
                    qtePosition.Add(-1);
            }
        
            for (int i = 0; i < beats.Count; i++)
            {
                float nextBeat = beats[i];
        
                if (nextBeat <= 0)
                {
                    nextBeat = 1;
                }
        
                if(isLongNoteDoing) //롱노트 처리 중엔 시간만 넘기기 //생성 X
                {
                    if (isLongNote[i])
                    {
                        isLongNoteDoing = false;
                        //isHolding = false;
                    }
        
                    yield return new WaitForSeconds((60f / bpm) / nextBeat);
                    continue;
                }
        
                yield return new WaitForSeconds((60f / bpm) / nextBeat);
                if (isLongNote[i]) //롱노트 시작
                {
                    qte = Instantiate(qteLongPrefabs, canvas.transform).GetComponent<QTELong>();
        
                    //롱 노트 처리
                    float holdingTime = 0f; 
                    for(int j = i + 1; j < beats.Count; j++)
                    {
                        holdingTime += (60f / bpm) / beats[j];
                        ((QTELong)qte).holdingCheckTime.Add(holdingTime);
                        if (isLongNote[j])
                        {
                            if(j ==  beats.Count - 1)
                                isAllNoteEnd = true;
                            break;
                        }
                    }
        
                    ((QTELong)qte).holdingTime = holdingTime;
                    isLongNoteDoing = true;
                }
                else //일반 노트
                {
                    qte = Instantiate(qtePrefabs, canvas.transform).GetComponent<QTEShort>();
                }
        
                qteList.Add(qte);
                if (qtePosition[i] < 0)
                    randPos = Random.Range(0, row * col);
                else
                    randPos = qtePosition[i];
        
                if (randPos >= row * col)
                    randPos = randPos % (row * col);
                
                qte.transform.position = new Vector2(rootPositionX + (randPos % row) * gapX, rootPositionY + (randPos / row) * gapY);
        
                qte.manager = this;
                qte.isPointNotes = pointNoteList[i];
                
                if (bpm <= 0)
                {
                    bpm = 120f; //default
                }
            }
            isAllNoteEnd = true;
            if (qteList.Count == 0)
            {
                RhythmManager.Instance.isPlaying = false;
            }
 }

```
        
일반 노트인 경우는 한 번 쉬고 생성
        
롱노트인 경우는 롱노트가 끝나는 시간까지 쉬고 나서 생성을 해준다.
        
yield return new WaitForSeconds((60f / bpm) / nextBeat);
        
을 통해 박자 사이마다 실행하는 시간동안 쉬게해준다.
        
 일반 노트인 경우는 한 번 쉬고 생성
        
롱노트인 경우는 롱노트가 끝나는 시간까지 쉬고 나서 생성을 해준다.
        
![image 19](https://github.com/user-attachments/assets/33186127-2513-4abd-bfa3-01c4542d961e)

        
모든 노트를 생성하고 qteList에 모든 QTE를 처리를 처리를 하면 RhythManager에 끝났음을 알린다.

</div>
</details>

</div>
</details>

<details>
<summary> 고스트 리듬게임 구현</summary>
<div markdown="1">

[리듬 박자 계산]
![image 20](https://github.com/user-attachments/assets/e06700b9-0d1f-4610-abf6-5587cd98a6f0)

    
Player Trans) 고스트가 생성될 상위 오브젝트
    
Direction) 고스트가 생성될 방향
    
Rotate Angle) 생성된 고스트의 회전값
    
Ghost Gaps) 고스트 간의 거리 (1의 경우 1초에 1m)
    
Ghost Original) 고스트를 만들 오리지널 오브젝트
    
Ghost Clip) 고스트가 특정 시간에 취할 행동을 위한 애니메이션 클립

    
<details>
<summary>고스트의 생성</summary>
<div markdown="1">

 ![image 21](https://github.com/user-attachments/assets/4293e960-8006-4255-b933-370b8e9c76a4)

비트 배열을 받아서 실제로 판정을 처리할 시간을 저장할 배열 생성
        
![애니메이션 길이를 리듬 전체에 맞추기 위해 재생속도를 변경하는 코드]

![image 22](https://github.com/user-attachments/assets/50101b95-684d-4007-8542-3783440f3611)

애니메이션 길이를 리듬 전체에 맞추기 위해 재생속도를 변경하는 코드
        
애니메이션이 반복되어야 하는 경우는 false 아닌경우 true로 설정

</div>
</details>

<details>
<summary>노란 외곽선의 판정 고스트 생성</summary>
<div markdown="1">

![image 23](https://github.com/user-attachments/assets/869304a8-5baa-4182-a4e6-f996aff1c622)

특정 방향으로 gap과 비트를 받아서 고스트의 위치 생성

![image 24](https://github.com/user-attachments/assets/46c0cce9-1dfb-404f-87a7-69b49dbec6ad)

위치 생성 후 애니메이션의 특정 시간의 동작을 적용
        
![image 25](https://github.com/user-attachments/assets/65d1f6f9-02ec-45b3-8117-2e6207d926c8)

그 후 머테리얼을 적용해서 반투명하게 설정
        
- 노란 외관선의 판정 고스트 생성
        
![image 26](https://github.com/user-attachments/assets/1d1c4d36-8817-4a8c-a571-f0eca2e7426e)

처음은 0번째 고스트와 동일한 위치 및 동작
        
머테리얼은 똑같이 적용

</div>
</details>

</div>
</details>


<details>
<summary>블럭 연결선</summary>
<div markdown="1">

- 블럭 연결

<details>
<summary> NavMesh를 사용하여 블럭 간 최단 거리를 LineRenderer로 연결 </summary>
<div markdown="1">

```csharp
            public void DrawLines()
            {
                if (!gameObject.activeSelf) gameObject.SetActive(true);
                lineRenderer.positionCount = 0;
                elements = TimelineManager.Instance.PlacedBlocks;
                List<Vector3> fullPathPoints = new();
                if (elements.Count <= 1) return;
                for (int i = 0; i < elements.Count - 1; i++)
                {
                    Vector3 start = elements[i].transform.position;
                    Vector3 end = elements[i + 1].transform.position;
            
                    NavMeshPath path = new();
            
                    if (NavMesh.CalculatePath(start, end, NavMesh.AllAreas, path))
                    {
                        for (int j = 0; j < path.corners.Length - 1; j++)
                        {
                            var preciseSegment = GetPreciseNavMeshLine(path.corners[j], path.corners[j + 1], 0.2f); // NavMesh 위 경로 추출
                            fullPathPoints.AddRange(preciseSegment);
                        }
                    }
                    else Debug.LogWarning($"[PATH FAIL] from {start} to {end}");
                }
            
                if (elements.Count > 0) fullPathPoints.Add(elements[^1].transform.position);
            
                lineRenderer.positionCount = fullPathPoints.Count;
                lineRenderer.SetPositions(fullPathPoints.ToArray());
            
            }
            
```

</div>
</details>

<details>
<summary>경로의 꺾인 부분을 연결해주는 거라, `GetPreciseNavMeshLine()`으로 높이 차이가 있을 때 바닥을 뚫는 현상 방지 </summary>
<div markdown="1">

```csharp
            List<Vector3> GetPreciseNavMeshLine(Vector3 from, Vector3 to, float step = 0.2f)
            {
                List<Vector3> pathPoints = new();
            
                float dist = Vector3.Distance(from, to);
                int steps = Mathf.CeilToInt(dist / step);
            
                for (int i = 0; i <= steps; i++)
                {
                    float t = i / (float)steps;
                    Vector3 rawPoint = Vector3.Lerp(from, to, t);
            
                    // NavMesh 위 위치 찾기
                    if (NavMesh.SamplePosition(rawPoint, out var hit, 1.0f, NavMesh.AllAreas))
                    {
                        pathPoints.Add(hit.position); // 정확히 NavMesh 위
                    }
                }
            
                return pathPoints;
            }
```

</div>
</details>


<details>
<summary>라인이 2D라 카메라를 따라 회전시켜 모든 방향에서도 잘 보이도록 설정</summary>
<div markdown="1">

```csharp
lineRenderer.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward)
```

</div>
</details>

</div>
</details>


<details>
<summary>블럭 조합</summary>
<div markdown="1">

 - 블럭 구성
        
   ![image 27](https://github.com/user-attachments/assets/90dfc6f6-4c63-45fb-9197-63d6c2202c26)

   ![image 28](https://github.com/user-attachments/assets/b08942e4-93fa-41e6-981a-b99dfcd4b85d)

        
    - 블럭의 조합 규칙을 담는 `CombineRule` 클래스를 만들어 데이터 로드
    - UGS(Unity Google Sheet) 오픈소스를 사용해 데이터 관리
        
```csharp
        public class CombineRule
        {
            public CombineType RuleType;
            public BlockType AllowedType; // 유형 허용
            public List<int> AllowedBlocksIds = new(); // 특정 블럭 허용
        }
        
        [Type(typeof(CombineRule), new string[] {"CombineRule"})]
        public class CombineRuleType : IType
        {
            public object DefaultValue => null;
        
            public object Read(string value)
            {
                string[] split = value.Split(',');
                List<int> idList = new List<int>();
                for (int i = 2; i < split.Length; i++)
                {
                    split[i] = split[i].Replace("[", string.Empty).Replace("]", string.Empty);
        
                    if (int.TryParse(split[i].Trim(), out int id))
                    {
                        idList.Add(id);
                    }
                    
                }
        
                return new CombineRule()
                {
                    RuleType = (CombineType)Enum.Parse(typeof(CombineType), split[0]),
                    AllowedType = (BlockType)Enum.Parse(typeof(BlockType), split[1]),
                    AllowedBlocksIds = idList
        
                };
            }
        
        }
        
```
        
- 규칙 검사
        
  블럭 상호작용 시 리스트에 넣어 조합 검사

<details>
<summary>`ValidationCombinations()`</summary>
<div markdown="1">

    ```csharp
            public void ValidateCombinations()
            {
                List<Block> blockList = new List<Block>(ReturnBlocks());
                if (blockList.Count == 0) return;
            
                foreach (var block in blockList)
                {
                    if (block.IsDeathTrigger) continue;
                    block.IsSuccess = false;
                }
            
                List<Block> availableBlocks = new List<Block>(blockList);
                foreach (var block in blockList)
                {
                    availableBlocks.Add(block);
                }
            
                for (int i = 0; i < blockList.Count; i++)
                {
                    Block current = blockList[i];
                    if (!availableBlocks.Contains(current)) continue;
            
                    bool success = true;
                    Block prevSuccess = null;
                    Block nextSuccess = null;
            
                    // 접촉 블럭 특수 규칙 처리
                    if (current is ContactBlock contact)
                    {
                        ContactBlockValid(contact, i);
                        continue;
                    }
            
                    // 선행 조합 검사 (앞 블럭만)
                    if (current.PreCombineRule != null && current.PreCombineRule.RuleType != CombineType.None)
                    {
                        success = false;
                        for (int j = 0; j < i; j++)
                        {
                            Block other = blockList[j];
                            if (!availableBlocks.Contains(other)) continue;
            
                            if (BlockValidator.CanCombineWithPrev(current, other))
                            {
                                prevSuccess = other;
                                success = true;
                                break;
                            }
                        }
            
                        if (!success && BlockValidator.RequiresPrevBlock(current))
                        {
                            current.IsSuccess = false;
                            continue;
                        }
                    }
            
                    // 후속 조합 검사 (뒤 블럭만)
                    if (current.NextCombineRule != null && current.NextCombineRule.RuleType != CombineType.None)
                    {
                        success = false;
                        for (int j = i + 1; j < PlacedBlocks.Count; j++)
                        {
                            Block other = blockList[j];
                            if (!availableBlocks.Contains(other)) continue;
            
                            if (BlockValidator.CanCombineWithNext(current, other))
                            {
                                nextSuccess = other;
                                success = true;
                                break;
                            }
                        }
            
                        if (!success && BlockValidator.RequiresNextBlock(current))
                        {
                            current.IsSuccess = false;
                            continue;
                        }
                    }
            
                    // 조합 성공 처리
                    current.IsSuccess = true;
                    if (prevSuccess != null)
                    {
                        prevSuccess.IsSuccess = true;
                        Debug.Log($"[{prevSuccess.Name}] + [{current.Name}] 조합 결과: 성공");
                    }
                    if (nextSuccess != null)
                    {
                        nextSuccess.IsSuccess = true;
                        Debug.Log($"[{current.Name}] + [{nextSuccess.Name}] 조합 결과: 성공");
                    }
            
                    current.SetGhost();
                    prevSuccess?.SetGhost();
                    nextSuccess?.SetGhost();
            
                    // 사용된 블럭 available에서 제거
                    availableBlocks.Remove(current);
                    if (prevSuccess != null) availableBlocks.Remove(prevSuccess);
                    if (nextSuccess != null) availableBlocks.Remove(nextSuccess);
                }
            
                // 실패 처리
                foreach (var block in blockList)
                {
                    if (!block.IsSuccess)
                    {
                        block.SetGhost();
                        Debug.Log($"[{block.Name}] 조합 결과: 실패");
                    }
                }
            }
            ```

</div>
</details>

- 블럭 위치 이동
 상호작용 한 블럭의 순서에 따라 정보 변경

<details>
<summary>`MoveBlockAndShift()`</summary>
<div markdown="1">

 ```csharp
            public void MoveBlockAndShift(int fromIndex, int toIndex)
            {
                if (fromIndex < 0 || fromIndex >= PlacedBlocks.Count) return;
            
                if (toIndex < 0) toIndex = 0;
                if (toIndex >= PlacedBlocks.Count) toIndex = PlacedBlocks.Count - 1;
            
                TimelineElement blockToMove = PlacedBlocks[fromIndex];
                PlacedBlocks.RemoveAt(fromIndex);
                PlacedBlocks.Insert(toIndex, blockToMove);
            
                for(int i = 0; i < slots.Count; i++)
                {
                    if(i < PlacedBlocks.Count)
                        slots[i].currentItem.Initialize(PlacedBlocks[i]);
                    else
                    {
                        if(slots[i].currentItem != null)
                        {
                            Destroy(slots[i].currentItem.gameObject);
                            slots[i].currentItem = null;
                        }
                    }
                }
            }
            
```

</div>
</details>

</div>
</details>

<details>
<summary>NPC FSM</summary>
<div markdown="1">

<details>
<summary>기본 행동 BaseState</summary>
<div markdown="1">

 ```csharp
        if (GameManager.Instance.SelectedBGM != null)
        {
        
        	 if (stateMachine.npc.isColliding)
           {
        	   StopAnimation(stateMachine.npc.AnimationData.WalkParameterHash);
             stateMachine.npc.Agent.isStopped = true;
           }
           else
           {
               stateMachine.npc.Agent.isStopped = false;
               moveTimer += Time.deltaTime;
               if (moveTimer >= stateMachine.npc.moveDelay)
               {
                   Move();
                   moveTimer = 0f;
               }
           }
           var agent = stateMachine.npc.Agent;
           bool isMoving = !agent.pathPending && agent.remainingDistance > agent.stoppingDistance;
           if (isMoving)
           {
               RotateVelocity();
               StartAnimation(stateMachine.npc.AnimationData.WalkParameterHash);
           }
           else StopAnimation(stateMachine.npc.AnimationData.WalkParameterHash);
        }
        else
        {
           StopAnimation(stateMachine.npc.AnimationData.WalkParameterHash);
           stateMachine.npc.Agent.isStopped = true;
        }
  ```

</div>
</details>

</div>
</details>
 - `Move()` 에서 랜덤 이동, 방향에 따른 애니메이션 출력
<details>
<summary>`Move()`</summary>
<div markdown="1">

  ```csharp
                public void Move()
                {
                    Transform npc = stateMachine.npc.transform;
                    Vector3 nextPosition = GetRandomPointInArea(stateMachine.npc.Area);
                
                    Vector3 forward = npc.forward;
                    Vector3 nextDir = (nextPosition - npc.position).normalized;
                    float crossY = Vector3.Cross(forward, nextDir).y;
                
                    if(Mathf.Abs(crossY) > 0.01f)
                    {
                	    if (crossY > 0f) StartAnimation(stateMachine.npc.AnimationData.TurnLeftParameterHash);
                      else StartAnimation(stateMachine.npc.AnimationData.TurnRightParameterHash);
                    }
                    StopAnimation(stateMachine.npc.AnimationData.TurnRightParameterHash);
                    StopAnimation(stateMachine.npc.AnimationData.TurnLeftParameterHash);
                    stateMachine.npc.Agent.SetDestination(nextPosition);
                }
   ```
<details>
<summary>IdleState</summary>
<div markdown="1">

```csharp
        if (stateMachine.npc is Guard)
        {
            if (stateMachine.npc.behaviorType != BaseBehaviorType.Idle)
            {
                GuardWait();
            }
            else GuardIdle();
        
        }
        else if (stateMachine.npc.behaviorType == BaseBehaviorType.Idle)
        {
            TalkingIdle();
        }
        else base.Update();
        
        if (!stateMachine.npc.IsAction)
        {
            if (stateMachine.npc.CurAlertTime > 0)
                stateMachine.npc.CurAlertTime -= Time.deltaTime;
            else DecreaseSuspicion();
        
            if (IsPlayerInSight())
            {
                stateMachine.ChangeState(stateMachine.AlertState);
            }
        }
 ```

</div>
</details>
- NPC 유형 별로 나눠 행동 제어
<details>
<summary>Guard의 경우 일반 NPC와 특정 지점을 번갈아가며 이동한다</summary>
<div markdown="1">

```csharp
                public void GuardWait()
                {
                    var agent = stateMachine.npc.Agent;
                
                    bool isMoving = !agent.pathPending && agent.remainingDistance > agent.stoppingDistance;
                    if (GameManager.Instance.SelectedBGM != null)
                    {
                        agent.updateRotation = false;
                        if (!isWaiting)
                        {
                            waitTimer += Time.deltaTime;
                
                            if (isMoving)
                            {
                                RotateVelocity();
                                StartAnimation(stateMachine.npc.AnimationData.WalkParameterHash);
                                StopAnimation(stateMachine.npc.AnimationData.LookAroundParameterHash);
                            }
                            else
                            {
                                StopAnimation(stateMachine.npc.AnimationData.WalkParameterHash);
                                cooldownTimer += Time.deltaTime;
                            }
                
                            if (cooldownTimer >= 2f)
                            {
                                if (waitTimer >= 3f)
                                {
                                    if (stateMachine.npc is Guard guard)
                                    {
                                        agent.SetDestination(guard.GetWaitPosition().transform.position);
                                        isWaiting = true;
                                        waitTimer = 0f;
                                        cooldownTimer = 0f;
                                        StartAnimation(stateMachine.npc.AnimationData.WalkParameterHash);
                                        StopAnimation(stateMachine.npc.AnimationData.LookAroundParameterHash);
                                    }
                                }
                            }
                        }
                        else // 대기중
                        {
                            if (isMoving)
                            {
                                RotateVelocity();
                                StartAnimation(stateMachine.npc.AnimationData.WalkParameterHash);
                                StopAnimation(stateMachine.npc.AnimationData.LookAroundParameterHash);
                            }
                            else
                            {
                                StopAnimation(stateMachine.npc.AnimationData.WalkParameterHash);
                                StartAnimation(stateMachine.npc.AnimationData.LookAroundParameterHash);
                                cooldownTimer += Time.deltaTime;
                
                                if (cooldownTimer >= 5f)
                                {
                                    isWaiting = false;
                                    cooldownTimer = 0f;
                                    waitTimer = 0f;
                                    moveTimer = 0f;
                                    agent.SetDestination(GetRandomPointInArea(stateMachine.npc.Area));
                                }
                            }
                        }
                        agent.updateRotation = true;
                    }
                }
```

</div>
</details>

<details>
<summary>`IsPlayerInSight()` 에서 플레이어 감지 → 경계 상태로 전환</summary>
<div markdown="1">

   ```csharp
                public bool IsPlayerInSight() //true -> 경계
                {
                    Transform player = GameManager.Instance.Player.transform;
                    Vector3 directionPlayer = (player.position - stateMachine.npc.transform.position).normalized;
                    float angle = Vector3.Angle(stateMachine.npc.transform.forward, directionPlayer);
                
                    float distance = Vector3.Distance(stateMachine.npc.transform.position, player.position);
                    if (angle > stateMachine.npc.ViewAngle / 2f || distance > stateMachine.npc.ViewDistance)
                    {
                        return false;
                    }
                
                    //벽
                    Vector3 headPosition = stateMachine.npc.transform.position + new Vector3(0, 1.5f, 0);
                
                    Vector3 playerClosetPoint = stateMachine.npc.playerCollider.ClosestPoint(headPosition);
                
                    float sqrDistance = (playerClosetPoint - headPosition).sqrMagnitude;
                
                    Ray ray = new Ray(headPosition, directionPlayer);
                    RaycastHit[] hits = Physics.RaycastAll(ray, stateMachine.npc.ViewDistance, stateMachine.npc.layer);
                
                    if (hits.Length == 0)
                    {
                        return false;
                    }
                
                    Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
                
                    RaycastHit firstHit = hits[0];
                    if (firstHit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        if (GameManager.Instance.Player.isLockpick)
                        {
                            stateMachine.ChangeState(stateMachine.ActionState);
                        }
                        return true;
                    }
                
                    return false;
                }
  ```

</div>
</details>

<details>
<summary>경계 상태 AlertState</summary>
<div markdown="1">
 의심 수치가 0 이상일 때 경계 상태. 최대 수치가 되면 행동 반응 상태로 전환
```csharp
            if (IsPlayerInSight())
            {
                IncreaseSuspicion();
                if (stateMachine.npc.CurSuspicion == stateMachine.npc.SuspicionParams.maxValue)
                    stateMachine.ChangeState(stateMachine.ActionState);
            }
            else if (!isAlert)
            {
                DecreaseSuspicion();
                if (stateMachine.npc.CurSuspicion == 0) stateMachine.ChangeState(stateMachine.IdleState);
            }
 ```
</div>
</details>

<details>
<summary> 행동 반응 ActionState </summary>
<div markdown="1">

 ```csharp
        if (isMovingToTarget)
        {
            RotateVelocity();
            MoveToTarget();
            return;
        }
        if (IsPlayerInSight()) // 시야 내
        {
            if (!isPlayerInSight)
            {
                isPlayerInSight = true;
                lostSightTimer = 0f;
                stateMachine.npc.CurAlertTime = 0f; // 경계 시간 초기화
            }
        
            stateMachine.npc.CurAlertTime += Time.deltaTime; // 경계 시간 카운트
            ContiActionByType(); // 지속형 행동
        
            if (stateMachine.npc.CurAlertTime >= stateMachine.npc.MaxAlertTime && !isTriggered)
            {
                TriggerActionByType(); // 최대 경계 시간 초과 시 발동형 행동
            }
        }
        else // 시야 밖
        {
            if (isPlayerInSight)
            {
                isPlayerInSight = false;
                lostSightTimer = 0f;
            }
        
            lostSightTimer += Time.deltaTime;
            // 최소 경계 시간 동안 지속형 행동
            if (lostSightTimer < stateMachine.npc.MinAlertTime) ContiActionByType();
            else
            {
                isAlert = false;
                stateMachine.npc.Agent.isStopped = false;
                stateMachine.ChangeState(stateMachine.AlertState); // 최소 경계 시간 지나면 중단
            }
        }
 ```

<details>
<summary>NPC별 지정된 행동 패턴 수행</summary>
<div markdown="1">

![image 29](https://github.com/user-attachments/assets/31928a17-517d-471a-86fa-d7456146ef35)

            
```csharp
            private void ContiActionByType() // 지속형
            {
                if (isTriggered) return;
                ActionType type = stateMachine.npc.ContiAlertAction;
            
                switch (type)
                {
                    case ActionType.Chase:
                        ChasePlayer();
                        break;
                    case ActionType.Watch:
                        LookAtTarget();
                        break;
                    default:
                        stateMachine.ChangeState(stateMachine.AlertState);
                        break;
                }
            }
            private void TriggerActionByType() // 발동형
            {
                isTriggered = true;
                stateMachine.npc.IsAction = true;
                StopAnimation(stateMachine.npc.AnimationData.TurnLeftParameterHash);
                StopAnimation(stateMachine.npc.AnimationData.TurnRightParameterHash);
            
                ActionType type = stateMachine.npc.TriggerAlertAction;
            
                switch (type)
                {
                    case ActionType.Notify:
                        NotifyTarget();
                        break;
                    default:
                        break;
                }
            }
 ```

</div>
</details>

<details>
<summary>행동 패턴에 따라 게임 오버</summary>
<div markdown="1">

<details>
<summary>`ChasePlayer()`에서 플레이어 추격 성공 시 게임 오버</summary>
<div markdown="1">

 ```csharp
                private void ChasePlayer()
                {
                    stateMachine.npc.isWalking = false;
                    stateMachine.npc.Agent.isStopped = false;
                    StartAnimation(stateMachine.npc.AnimationData.RunParameterHash);
                    stateMachine.npc.Agent.SetDestination(stateMachine.Target.transform.position);
                    if (!stateMachine.npc.Agent.pathPending && stateMachine.npc.Agent.remainingDistance <= stateMachine.npc.Agent.stoppingDistance)
                    {
                        StopAnimation(stateMachine.npc.AnimationData.RunParameterHash);
                        GameManager.Instance.GameOver();
                        stateMachine.ChangeState(stateMachine.IdleState);
                    }
                }
 ```

<details>
<summary>`NotifyTarget()` 에서 NPC가 Target으로 이동하여 Target이 안전 구역으로 이동하면 게임 오버</summary>
<div markdown="1">

  ```csharp
                private void NotifyTarget()
                {
                    if (stateMachine.npc.target != null)
                    {
                        if (stateMachine.npc.isColliding) stateMachine.npc.Agent.isStopped = true;
                        else stateMachine.npc.Agent.isStopped = false;
                        stateMachine.npc.Agent.SetDestination(stateMachine.npc.target.transform.position);
                        StartAnimation(stateMachine.npc.AnimationData.RunParameterHash);
                        stateMachine.npc.isWalking = false;
                        isMovingToTarget = true;
                    }
                }
                
                private void MoveToTarget()
                {
                    if (stateMachine.npc.target.IsNotified || hasNotified) return;
                    var agent = stateMachine.npc.Agent;
                    agent.speed = 4f;
                
                    Vector3 curTargetPos = stateMachine.npc.target.transform.position;
                
                    if (!agent.pathPending && Vector3.Distance(lastDestination, curTargetPos) > 0.5f)
                    {
                        agent.SetDestination(curTargetPos);
                        lastDestination = curTargetPos;
                    }
                    if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) //도착시
                    {
                        if (!hasOpenDoor && stateMachine.npc is Friend friend)
                        {
                            hasOpenDoor = true;
                            agent.isStopped = true;
                            friend.door.OpenDoor();
                            agent.isStopped = false;
                            agent.SetDestination(stateMachine.npc.target.transform.position);
                            return;
                        }
                        StartAnimation(stateMachine.npc.AnimationData.NotifyParameterHash);
                        StopAnimation(stateMachine.npc.AnimationData.RunParameterHash);
                        Vector3 lookDir = (stateMachine.npc.target.transform.position - stateMachine.npc.transform.position);
                        lookDir.y = 0;
                        if (lookDir.sqrMagnitude > 0.01f)
                        {
                            Quaternion lookRot = Quaternion.LookRotation(lookDir);
                            Quaternion rotated = lookRot * Quaternion.Euler(0, -90f, 0);
                            stateMachine.npc.transform.rotation = rotated;
                        }
                        agent.isStopped = true;
                        isMovingToTarget = false;
                
                        if (stateMachine.npc is Friend f)
                        {
                            if (f.IsNotifying) return;
                            f.NotifyTarget(stateMachine.npc.target, () =>
                            {
                                StopAnimation(stateMachine.npc.AnimationData.NotifyParameterHash);
                            });
                
                        }
                    }
                    else
                    {
                        agent.isStopped = false;
                    }
                }
 ```

</div>
</details>
</div>
</details>

</div>
</details>

</div>
</details>

</div>
</details>

---

## ▪️최적화

<details>
<summary>오클루젼 컬링</summary>
<div markdown="1">
    
![occlusion_Culling_mask](https://github.com/user-attachments/assets/e793c7ac-88e1-491e-8bee-f1717ff9a7af)

![ocullison_culling_mask_1](https://github.com/user-attachments/assets/ad3ebbd9-28dd-44be-b452-08caa86a4992)


- 레벨의 지오메트리를 나눠서 유니티의 해당 카메라의 절두체로 화면을 랜더링하는 기술인 오클루젼 컬링을 사용하였습니다
  
- 해당 기능을 사용하여, 3D 오브젝트가 가장 집중적으로 렌더링 되는 시점에서 3D 배치 수를 크게 줄이고, FPS를 크게 향상시킬 수 있었습니다
- 
- `Visualize` 기능을 사용하여, 씬에 있는 오브젝트를 조절하여 적절한 오클루젼 컬링을 적용하였습니다

</div>
</details>

---

## ▪️트러블 슈팅
<details>
<summary>블럭 중복 조합 현상</summary>
<div markdown="1">

- **현상**
        - 이미 성공 처리된 블럭이 뒤에 오는 블럭과 다시 조합되어 결과가 반영되고 있었다.
- **해결**
        - 배치된 블럭의 리스트를 새로 만들어 검사한 블럭의 조합과 맞춰진 블럭은 리스트에서 제거하고 그 후에 나오는 블럭과의 조합 검사를 막아 해결

</div>
</details>

<details>
<summary>라인이 맵 뚫는 현상</summary>
<div markdown="1">

- 라인이 맵 뚫는 현상
    - **현상**
        - `NavMesh.CalculatePath()` 로 계산한 경로를 `LineRenderer`로 그렸더니 경로가 지형을 뚫고 내려가거나 공중에 떠 있는 라인이 그려졌다.
          
    - **해결**
        - `NavMesh.CalculatePath()`로 `path.corners` 추출 한 뒤 각 `corner` 구간 사이를 일정 간격으로 촘촘하게 `Lerp` 보간
          
        1. 보간된 위치마다 `NavMesh.SamplePosition()`으로 정확히 NavMesh 위 위치로 보정
            
            ```csharp
            List<Vector3> GetPreciseNavMeshLine(Vector3 from, Vector3 to, float spacing = 0.2f)
            {
                List<Vector3> result = new();
                float dist = Vector3.Distance(from, to);
                int steps = Mathf.CeilToInt(dist / spacing);
            
                for (int i = 0; i <= steps; i++)
                {
                    float t = (float)i / steps;
                    Vector3 raw = Vector3.Lerp(from, to, t);
            
                    if (NavMesh.SamplePosition(raw + Vector3.up * 0.5f, out var hit, 1f, NavMesh.AllAreas))
                        result.Add(hit.position);
                    else
                        result.Add(raw);
                }
            
                return result;
            }
            ```
 2. `LineRenderer.SetPositions()`에 이 결과를 사용

</div>
</details>


<details>
<summary>드래그 상호작용 오브젝트가 드래그 아웃이 됐을 때, 재위치 하지 않는 현상</summary>
<div markdown="1">

 - **현상**
        - `OnDrop` 메서드가 드래그 하던 아이템을 원래 위치로 돌려야 하는데, 기능이 안됨
 - **해결**
        1. `OnEndDrag`와 `OnDrop` 메서드를 따로 분리
        2. `OnDrop` 에서는 판정을 확실히 하여 슬롯 데이터와 위치를 바꿔주고, `OnEndDrag`에서는 원래 자리로 재위치 해줌 

```csharp
            using UnityEngine;
            using UnityEngine.EventSystems;
            
            public class UI_Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler,IPointerEnterHandler,IPointerExitHandler 
            {
                public UI_Sequence currentItem; // 슬롯 안에 들어있는 아이템 (프리팹 인스턴스)
            
                private Transform originalParent; // 드래그 시작할 때 아이템이 원래 어디에 있었는지, 기억하려고 사용
                private Canvas canvas; // 드래그 중에 아이템 따라다니게 할 때 필요
            
                public int slotIndex; // 시퀀스가 어느 슬롯에 생성될 지 확인 시 필요
            
                private void Start()
                {
                    canvas = GetComponentInParent<Canvas>(); // 자신의 부모중 canvas를 찾아서 저장한다
                }
            
                public void OnBeginDrag(PointerEventData eventData)
                {
                    if (currentItem != null)
                    {
                        originalParent = currentItem.transform.parent; // 현재 부모 저장
                        currentItem.transform.SetParent(canvas.transform); // 캔버스 위로 올림
                        currentItem.GetComponent<CanvasGroup>().blocksRaycasts = false; // 드래그 중엔 Raycast 막기
                    }
                }
            
                public void OnDrag(PointerEventData eventData) 
                {
                    if (currentItem != null)
                    {
                        currentItem.transform.position = eventData.position; // 마우스 따라다니게
                    }
                    if (currentItem != null)
                        currentItem.SetOutline(true);
                }
            
                public void OnEndDrag(PointerEventData eventData)
                {
                    if (currentItem != null)
                    {
                        currentItem.transform.SetParent(originalParent); // 원래 자리로 돌림
                        currentItem.transform.localPosition = Vector3.zero;
                        currentItem.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    }
                    if (currentItem != null)
                        currentItem.SetOutline(false);
                }
            
                public void OnDrop(PointerEventData eventData)
                {
                    if (eventData.pointerDrag != null)
                    {
                        UI_Slot otherSlot = eventData.pointerDrag.GetComponentInParent<UI_Slot>();
                        if (otherSlot != null)
                        {
                            TimelineManager.Instance.MoveBlockAndShift(otherSlot.slotIndex,slotIndex);
                            for (int i = 0; i < TimelineManager.Instance.PlacedBlocks.Count; i++)
                            {
                                Debug.Log($"[정렬후] slot {i} = {(TimelineManager.Instance.PlacedBlocks[i] != null ? TimelineManager.Instance.PlacedBlocks[i].Name : "null")}");
                            }
                        }
                        TimelineManager.Instance.OnBlockUpdate?.Invoke();
                    }
                }
```
</div>
</details>

<details>
<summary>애니메이션 처리</summary>
<div markdown="1">

 **현상**
- 애니메이션의 포지션이 로컬포지션을 변경이 되어버리는 현상
        
![8ddb798f-a94c-416f-8eac-c089eba160c8](https://github.com/user-attachments/assets/9d21edd1-aa2b-4553-88d5-61a766c81499)

        
 **해결**
        
![image 30](https://github.com/user-attachments/assets/001bf44c-1102-456c-b0af-684da37bdf1c)

- 로컬 포지션이 변하더라도 글로벌포지션을 유지할 수 있게 빈 오브젝트 밑에 추가를 하였다.

</div>
</details>

<details>
<summary>로드 씬과 초기값</summary>
<div markdown="1">

**현상**
        - 값을 전부 넣은 뒤 씬이 로드가 되어 미리 계산된 값이 씬에 저장된 값으로 변경이 되어버리는 문제
        - 값이 전부 들어갔지만 씬이 로드가 되는 순간 갑자기 값이 전부 초기화가 되어버렸다.
            
![image 31](https://github.com/user-attachments/assets/f3235e7c-bcbc-4ff4-bf6c-ac06f148d6aa)

            
**해결**
        - 씬을 로드한 이후 값을 가져오는 방식으로 변경
        
![image 32](https://github.com/user-attachments/assets/5a8e05fd-38aa-460a-a89f-a657afacbe98)

</div>
</details>

<details>
<summary>상호 작용</summary>
<div markdown="1">

**현상**
        - 콜라이더 내부에 여러 상호작용 가능한 오브젝트들이 있을 때 원하는 오브젝트가 상호작용 안되는 문제
**해결**
        - 콜라이더에 들어간 모든 상호작용 가능한 오브젝트들을 리스트에 추가 후 콜라이더에 벗어난 상호작용 가능한 오브젝트들은 리스트에서 제거
        - 콜라이더 범위 안에 들어간 상호작용 가능한 오브젝트들 중 플레이어와 가장 가까운 오브젝트를 상호작용 할 수 있게하여 해결하였습니다.
        
![image 33](https://github.com/user-attachments/assets/a92f3e75-5471-459a-befb-6648abfb9b83)

</div>
</details>

---
## ▪️에셋 링크

- **캐릭터**
  
https://vroid.com/en

- **맵**
  
https://assetstore.unity.com/packages/3d/environments/simple-modular-pillars-255707
https://assetstore.unity.com/packages/2d/textures-materials/sky/free-stylized-skybox-212257
https://assetstore.unity.com/packages/3d/environments/horror-mansion-254104

- **애니메이션**
  
https://www.mixamo.com/#/?page=1&type=Character

- **이미지 및 영상 소스**
  
https://www.midjourney.com/help
https://openai.com/
---

## ▪️**외부 리소스**

- https://github.com/mob-sakai/ParticleEffectForUGUI.git
- https://github.com/shlifedev/uni-google-sheets.git?path=src/
