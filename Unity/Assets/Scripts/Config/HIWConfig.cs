using UnityEngine;

[CreateAssetMenu(fileName = "HIWConfig", menuName = "Communities/Configuration")]
public class HIWConfig : ScriptableObject
{
    [Header("Parameters")] 
    [SerializeField] [Tooltip("Api url")]
    public string apiBaseUrl;
}

