using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class InputManager : UnitySingleton<InputManager>
{
    #region Variables

    //public double HoverTimerSeconds = 1.0f;

    Ray m_InputRay;

    RaycastHit m_RayHitInfo;

    bool m_Result;

    GameObject m_LastHit;

    GameObject m_CurrentHit;

    GameObject m_LastMouseDownObject;

    #endregion


    // Use this for initialization
    void Start () {
        m_RayHitInfo = new RaycastHit();
        m_LastMouseDownObject = null;
        m_LastHit = null;
        m_CurrentHit = null;
        m_InputRay = new Ray();
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_InputRay = Camera.main.ScreenPointToRay(CrossPlatformInputManager.mousePosition);
        m_Result = Physics.Raycast(m_InputRay, out m_RayHitInfo, LayerMask.NameToLayer("Input"));
        if (!m_Result)
        {
            if( m_LastHit != null )
            {
                m_LastHit.SendMessage("MouseExit");
            }
            m_LastHit = null;
            return;
        }

        m_CurrentHit = m_RayHitInfo.collider.gameObject;
        if( m_LastHit != m_CurrentHit )
        {
            m_CurrentHit.SendMessage("MouseEnter");
        }
        bool btnDown = GetMouseDown();
        if( btnDown )
        {
            m_LastMouseDownObject = m_CurrentHit;
            m_CurrentHit.SendMessage("MouseDown");
        }

        bool btnUp = GetMouseUp();
        if( btnUp )
        {
            m_CurrentHit.SendMessage("MouseUp");

            if ( m_LastMouseDownObject == m_CurrentHit)
            {
                m_CurrentHit.SendMessage("MouseClick");
            }
            m_LastMouseDownObject = null;
        }

        m_LastHit = m_CurrentHit;
    }

    bool GetMouseDown(int button = 0)
    {
        return Input.GetMouseButtonDown(button);
    }

    bool GetMouse(int button = 0)
    {
        return Input.GetMouseButton(button);
    }

    bool GetMouseUp(int button = 0)
    {
        return Input.GetMouseButtonUp(button);
    }
}


public interface IMouseListener
{
    //void MouseHover();
    void MouseEnter();
    void MouseExit();
    void MouseDown();
    void MouseClick();
    void MouseUp();
}