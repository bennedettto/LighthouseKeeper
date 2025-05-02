# Lighthouse Keeper

## List of Game States

| Name                | Values | Description        |
|---------------------|--------|--------------------|
| constant.false      | 0      | Is always set to 0 |
| constant.true         | 1     | Is always set to 1 |
| boat.hasArrived     | 0, 1     | Wether the boat has arrived at the lighthouse         |
| boat.hasLeft        | 0, 1     | Whether the boat has left the lighthouse              |
| boat.wasRemoved     | 0, 1     | Whether the boat was removed from the game            |
| fuel.level          | 0 to 100 | Fuel level of the generator                            |
| generator.isRunning | 0, 1     | Is the light powered                                  |
| generator.doorOpen  | 0, 1, 2  | Whether the generator door is open (1=Open, 2=Closed) |
| letter.wasRead      | 0, 1     | Whether the introduction letter was read              |
| rain.IsRaining      | 0, 1     | Whether it is raining                                 |
| rain.amount         | 0 to 100 | Amount of rain                                        |
| tide.level          | 0, 1     | Current tide level                                    |
