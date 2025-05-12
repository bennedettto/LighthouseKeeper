# Lighthouse Keeper

## List of Game States

| Name                | Values   | Description                                                       |
|---------------------|----------|-------------------------------------------------------------------|
| constant.false      | 0        | Is always set to 0                                                |
| constant.true       | 1        | Is always set to 1                                                |
| boat.hasArrived     | 0, 1     | Whether the boat has arrived at the lighthouse                    |
| boat.hasLeft        | 0, 1     | Whether the boat has left the lighthouse                          |
| boat.wasRemoved     | 0, 1     | Whether the boat was removed from the game                        |
| fuel.level          | 0 to 100 | Fuel level of the generator                                       |
| generator.isRunning | 0, 1     | Is the light powered                                              |
| generator.doorOpen  | 0, 1, 2  | Whether the generator door is open (0=Initial, 1=Open, 2=Closed)  |
| item.fuelKey        | 0, 1     | has the fuel key (0=No, 1=Yes)                                    |
| item.tools          | 0, 1     | has tools (0=No, 1=Yes)                                           |
| letter.wasRead      | 0, 1     | Whether the introduction letter was read                          |
| rain.isRaining      | 0, 1     | Whether it is raining                                             |
| rain.amount         | 0 to 100 | Amount of rain                                                    |
| storage.doorOpen    | 0, 1, 2  | Whether the storage door is locked (0=Locked, 1=Unlocked, 2=Open) |
| tide.level          | 0, 1     | Current tide level                                                |
| time.isDaytime      | 0, 1     | Whether it is daytime                                             |
| time.dayNumber      | 0, 1, 2  | Current day number (0=Day 1, 1=Day 2, 2=Day 3)                    |

## Usage

Canvas_WithPergament
  DependsOnState
    Action: DestroyWhenMet
    Check: Once
    Target: None
    Conditions:
      - Key: letter.wasRead
        Equal
        Value: 1

Canvas_WithPergament
  FadeIn:
    DependsOnState:
      Action: EnableWhenMet
      Check: Always
      Target: None
      Conditions:
        - Key: letter.wasRead
          Equal
          Value: 1
  Pergament:
    InputListener:
      Key: letter.wasRead
      Value: 1

Canvas_NoPergament
  DependsOnState
    Action: DestroyWhenMet
    Check: Once
    Target: None
    Conditions:
      - Key: letter.wasRead
        Equal
        Value: 0

BOAT_ARRIVAL
  DependsOnState
    Action: EnableWhenMet
    Check: Always
    Target: None
    Conditions:
      - Key: letter.wasRead
        Equal
        Value: 1

  Timeline
    DependsOnState
      Action: DisableWhenMet
      Check: Always
      Target: PLAYER
      Conditions:
        - Key: boat.hasArrived
          Equal
          Value: 0
    SetGameStateInvokable
      Key: boat.hasArrived
      Value: 1
    SetGameStateInvokable
      Key: boat.hasLeft
      Value: 1
    SetGameStateInvokable
      Key: boat.wasRemoved
      Value: 1
  OverridePlayerPosition
    DependsOnState
      Action: EnableWhenMet
      Check: Always
      Target:
      Conditions:
        - Key: boat.hasArrived
          Equal
          Value: 1

BOAT
  Character1
    InvokeWhenGameState
      Invokable: [link]
      Conditions
        - Key: boat.hasArrived
          Equal
          Value: 1
    SetAnimatorStateInvokable
      Animator: [link]
      Key: Rest1
      ParamterType: Trigger
    InvokeWhenGameState
      Invokable: [link]
      Conditions
        - Key: boat.hasLeft
          Equal
          Value: 1
    SetAnimatorStateInvokable
      Animator: [link]
      Key: Paddle
      ParamterType: Trigger
  CinemachineCamera
    DependsOnState
      Action: DisableWhenMet
      Check: Always
      Target: None
      Conditions:
        - Key: boat.hasArrived
          Equal
          Value: 1

Postion
  X0,97  0,04
  Y1,28  0,13
  Z0     0
Rottion
  X
  Y
  Z

Canas_WithPergament
  DpendsOnState
   Action: DestroyWhenMet
   Check: Once
   Target: None
   Conditions:
     - Key: letter.wasRead
       Equal
       Value: 1

Canas_WithPergament
  FdeIn:
   DependsOnState:
     Action: EnableWhenMet
     Check: Always
     Target: None
     Conditions:
       - Key: letter.wasRead
         Equal
         Value: 1
  Prgament:
   InputListener:
     Key: letter.wasRead
     Value: 1

Canas_NoPergament
  DpendsOnState
   Action: DestroyWhenMet
   Check: Once
   Target: None
   Conditions:
     - Key: letter.wasRead
       Equal
       Value: 0

BOA_ARRIVAL
  DpendsOnState
   Action: EnableWhenMet
   Check: Always
   Target: None
   Conditions:
     - Key: letter.wasRead
       Equal
       Value: 1

  Tmeline
   DependsOnState
     Action: DisableWhenMet
     Check: Always
     Target: PLAYER
     Conditions:
       - Key: boat.hasArrived
         Equal
         Value: 0
   SetGameStateInvokable
     Key: boat.hasArrived
     Value: 1
   SetGameStateInvokable
     Key: boat.hasLeft
     Value: 1
   SetGameStateInvokable
     Key: boat.wasRemoved
     Value: 1
  OerridePlayerPosition
   DependsOnState
     Action: EnableWhenMet
     Check: Always
     Target:
     Conditions:
       - Key: boat.hasArrived
         Equal
         Value: 1

BOA
  Caracter1
   InvokeWhenGameState
     Invokable: [link]
     Conditions
       - Key: boat.hasArrived
         Equal
         Value: 1
   SetAnimatorStateInvokable
     Animator: [link]
     Key: Rest1
     ParamterType: Trigger
   InvokeWhenGameState
     Invokable: [link]
     Conditions
       - Key: boat.hasLeft
         Equal
         Value: 1
   SetAnimatorStateInvokable
     Animator: [link]
     Key: Paddle
     ParamterType: Trigger
  CnemachineCamera
   DependsOnState
     Action: DisableWhenMet
     Check: Always
     Target: None
     Conditions:
       - Key: boat.hasArrived
         Equal
         Value: 1








