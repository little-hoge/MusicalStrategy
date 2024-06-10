using UnityEngine;
public class ParentObject : MonoBehaviour       
{
    //更新処理をした後に呼ばれるUpdate
    void LateUpdate()
    {
        // 子オブジェクトが一つもない場合、親オブジェクトを破壊する
        if (transform.childCount == 0) Destroy(gameObject);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
