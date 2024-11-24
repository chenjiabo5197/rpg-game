# rpg-game
my first rpg game，base on unity

### 疑问
 1、设置ThunderStrike的图层为player层后，thunderStrike prefab的OnTriggerEnter2D函数中，collision.GetComponent<Enemy>() != null一直为false，将thunderStrike prefab的图层修改为default后正常，怀疑是否与通过IgnoreLayerCollision设置player与enemy图层之间忽略碰撞导致的

