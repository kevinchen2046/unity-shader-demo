using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class OutlineScript : PostEffectsBase
{
    public bool inBufferMode = false;

    [Range(0, 1)]
    public float OutlineWidth=1.0f;
    public Color OutlineColor;
    public float SimpleDistance=0.01f;

    public Shader curShader;

    private Material _cureMaterial;

    Material material
    {
        get
        {
            if (_cureMaterial == null)
            {
                _cureMaterial = new Material(curShader);
               // _cureMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return _cureMaterial;
        }
    }

    private CommandBuffer commandBuffer = null;

    private List<Renderer> outlinedObjects = new List<Renderer>();

    

    public bool Initialized
    {
        get { return commandBuffer != null; }
    }

    protected override void Start()
    {
        base.Start();
        _cureMaterial = new Material(curShader);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hitSelectable = Physics.Raycast(ray, out var hit) && hit.transform.CompareTag("Selectable");
            if (hitSelectable)
            {
                Renderer[] renderers = hit.transform.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    if (outlinedObjects.Contains(renderer))
                    {
                        outlinedObjects.Remove(renderer);
                    }
                    else
                    {
                        outlinedObjects.Add(renderer);
                    }
                }
            }
            else
            {
                outlinedObjects.Clear();
            }
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_cureMaterial != null)
        {
            _cureMaterial.SetFloat("_OutlineWidth", OutlineWidth);
            if(OutlineColor !=null) {
                OutlineColor.a = 1.0f;
                _cureMaterial.SetColor("_OutlineColor", OutlineColor);
            }
            _cureMaterial.SetFloat("_SimpleDistance", SimpleDistance);
        }

        commandBuffer = new CommandBuffer();
        commandBuffer.name = "Outline";

        //setup stuff
        int selectionBuffer = Shader.PropertyToID("_SelectionBuffer");
        commandBuffer.GetTemporaryRT(selectionBuffer, source.descriptor);

        //render selection buffer
        commandBuffer.SetRenderTarget(selectionBuffer);
        commandBuffer.ClearRenderTarget(true, true, Color.clear);
        if (outlinedObjects != null && outlinedObjects.Count > 0)
        {
            for (int i = 0; i < outlinedObjects.Count; i++)
            {
                commandBuffer.DrawRenderer(outlinedObjects[i], _cureMaterial);
            }
        }

        if (inBufferMode)
        {
            commandBuffer.Blit(selectionBuffer, destination);
        }
        else
        {
            //apply everything in commandbuffer
            commandBuffer.Blit(source, destination, _cureMaterial);
        }

        //clean up 
        commandBuffer.ReleaseTemporaryRT(selectionBuffer);

        //execute and clean up commandbuffer itself
        Graphics.ExecuteCommandBuffer(commandBuffer);
        commandBuffer.Dispose();
    }
}
