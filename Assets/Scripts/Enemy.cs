using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    /// <summary>  
    /// プレイヤー  
    /// </summary>  
    [SerializeField] private Player player_ = null;

    /// <summary>  
    /// ワールド行列   
    /// </summary>  
    private Matrix4x4 worldMatrix_ = Matrix4x4.identity;

    /// <summary>  
    /// ターゲットとして設定する  
    /// </summary>  
    /// <param name="enable">true:設定する / false:解除する</param>  
    public void SetTarget(bool enable)
    {
        // マテリアルの色を変更する  
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.materials[0].color = enable ? Color.red : Color.white;
    }

	/// <summary>
	/// 開始処理
	/// </summary>
	public void Start()
    {
		worldMatrix_ *= Matrix4x4.Translate(transform.position);
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    public void Update()
    {
		//索敵
		var normalz = new Vector3(0, 0, 1);
		var enemyForward = worldMatrix_ * normalz;
		var enemyViewCos = Mathf.Cos(Mathf.Deg2Rad * 20);//20度

		var enemyToPlayer = (player_.transform.position - transform.position).normalized;
		var dot = Vector3.Dot(enemyForward, enemyToPlayer);

		if (enemyViewCos <= dot)
		{
			//回転
			var rad = Mathf.Clamp(Mathf.Acos(dot), 0, Mathf.Deg2Rad * 10);

			var cross = Vector3.Cross(enemyForward, enemyToPlayer);

			rad *= (cross.y / Mathf.Abs(cross.y));//符号付きを絶対値で割る(1か-1になる)

			Matrix4x4 rotMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, Mathf.Rad2Deg * rad, 0));

			//移動
			Matrix4x4 transMatrix = Matrix4x4.Translate(new Vector3(0, 0, 0.2f));

			worldMatrix_ = worldMatrix_ * (transMatrix * rotMatrix);

			//最終移動
			transform.position = worldMatrix_.GetColumn(3);
			transform.rotation = worldMatrix_.rotation;
		}
	}
}
