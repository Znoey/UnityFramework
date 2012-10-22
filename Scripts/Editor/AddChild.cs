using UnityEngine;
using UnityEditor;
using System.Collections;
 
 
public class AddChild : ScriptableObject
{
 
    [MenuItem ("GameObject/+Add Child &n")]
    static void MenuAddChild()
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
		
        foreach(Transform transform in transforms)
        {
            GameObject newChild = new GameObject("_null");
            newChild.transform.parent = transform;
			newChild.transform.localPosition = Vector3.zero;
			Selection.activeGameObject = newChild;
        }
		
    }    
    
    [MenuItem ("GameObject/+Add Parent &p")]
    static void MenuInsertParent()
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel |
            SelectionMode.OnlyUserModifiable);
 
        GameObject newParent = new GameObject("_Parent");
        Transform newParentTransform = newParent.transform;
 
        if(transforms.Length == 1)
        {
            Transform originalParent = transforms[0].parent;
            transforms[0].parent = newParentTransform;
            if(originalParent)
                newParentTransform.parent = originalParent;
        }
        else
        {
            foreach(Transform transform in transforms)
                transform.parent = newParentTransform;
        }
    }
}