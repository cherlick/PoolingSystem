using UnityEngine;
using PoolingSystem;

public class TestObjects : MonoBehaviour
{
    private float _timer = 5;
    private void OnEnable() {
        
        _timer = 5;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(0,0.1f));
        _timer -= Time.deltaTime;
        if (_timer<0)
        {
            ObjectPooling.Instance.ReturnObject(gameObject, gameObject.name);
        }
    }
}
