# Proyecto-Moviles-2023
Patrones de Diseño:

- Composite: Assets/Scripts/Composite En el caso de los paneles en el LevevlSelector usa composite ya que eran muchas entidades
 que formaban panrte de la escena y todas tenian distintos tipos de funcionalidades, el boton del nivel abre el panel de los distintos tipos de dificultades y las dificultades tienen adentro la cantidad de estrellas y la cantidad de estrellas el puntaje maximo

- Pooling: Assets/Scripts/Gameplay/GameplayController.cs propiedad de private Queue<ChunkView> poolChunks aca tuve que usar obligatoriamente pooling ya que uso un sistema de chunks para spawnear los escenarios y si los mantengo todos instanciados la performance
en dispositivos moiviles baja drasticamente, y note una gran diferencia en usar esto aca

- Lightweight: Assets/Scripts/LevelSelectionsSO/LevelConfigSO.cs aca tuve que usar Lightweight ya que me permite tener poca informacion de un nivel en distintos SOs ya como la velocidad la cantidad de objetos por jaula la cantidad de jaulas por chunk que animal mostrar
que objeto mostrar y esto proveerselo al Gameplay para que el gameplay sea mas variado con los cambios de velocidad, tiempo, score, arte,etc 
