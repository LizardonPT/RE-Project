using Tools;
using UnityEngine;

public class ToolObject : MonoBehaviour
{
    [SerializeField]
    string toolName;
    public ITool Tool { get; private set; }

    void Awake()
    {
        Tool = Utilities.GenerateTool(toolName, gameObject);
    }
}
