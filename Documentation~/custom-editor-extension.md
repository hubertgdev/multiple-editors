# Muffin Dev for Unity - Multiple Editors Manager - `CustomEditorExtension`

Represents an editor extension for an actual Unity Editor instance that inherits from [`MultipleEditorsHandler`](multiple-editors-handler.md).

You must inherit from this class to create a custom editor extension and register it with the `MultipleEditorsManager`.

When you inherit from this class, you can use the same method names as overrides thet a regular `Editor` class uses like `OnEnable()`, `OnInpectorGUI()`, etc. You can also get the object being inspected by using `target`, `targets` or `serializedObject` accessors.

## Usage

In the following example, you'll see how to create a custom editor extension for `GameObject`:

```cs
using UnityEngine;
using UnityEditor;
using MuffinDev.EditorUtils.MultipleEditors;

// Use InitializeOnLoad attribute to ask for Unity to load this class after recompiling
[InitializeOnLoad]
public class GameObjectTestExtension : CustomEditorExtension<GameObject>
{
    // You must use the static constructor to register this custom editor extension
    static GameObjectTestExtension()
    {
        RegisterCustomEditor(() => { return new GameObjectTestExtension(); });
    }

    public override void OnHeaderGUI()
    {
        EditorGUILayout.HelpBox("Hello World!", MessageType.Info);
        EditorGUILayout.Space();
    }
}
```

![`CustomMultipleExtension` usage result](./Images/multiple-editors-manager-usage-gameobject.png)

There's some ready-to-use multiple editors managers in this module files, such as `GameObjectMultipleEditors`. This class uses `[CustomEditor(typeof(GameObject))]`, meaning it customize the `GameObject` editor. Since that class is already defined, you just have to create your own extension using the code above, and register it with the `MultipleEditorsManager` using the `RegisterCustomEditor()` method.

Of course, this works also with your own Objects. Note that the `CustomEditorExtension` class is generic, and the type to specify defines the target type of your custom editor extension. Here we want to target `GameObject`, but you can use `CustomEditorExtension<MyBehaviour>` to target a custom type.

**Note that register a custom editor extension must be done in the static constructor of your extensions.**

## Methods

```cs
protected static void RegisterCustomEditor(MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod);
protected static void RegisterCustomEditor(MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod, CustomEditorExtensionOptions _Options);
protected static void RegisterCustomEditor(MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod, int _Order, bool _RequiresConstantRepaint = false);
protected static void RegisterCustomEditor(MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod, int _Order, string _DisplayName, bool _RequiresConstantRepaint = false);
protected static void RegisterCustomEditor(MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod, int _Order, string _DisplayName, string _Description, bool _RequiresConstantRepaint = false);
protected static void RegisterCustomEditor(MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod, string _DisplayName, bool _RequiresConstantRepaint = false);
protected static void RegisterCustomEditor(MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod, string _DisplayName, string _Description, bool _RequiresConstantRepaint = false);
```

Registers this custom editor with the manager to make it available for multiple editors system.

- `MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod`: The method to use for creating an instance of this custom editor extension. For example if your custom editor class name is `TestEditorExtension`, you can use `RegisterCustomEditor(() => { return new TestEditorExtension(); });`
- `CustomEditorExtensionOptions _Options`: The options and default values to use for the `CustomEditorExtension` instance
- `int _Order`: The position of the editor in the inspector. The higher the value, the highest the editor is drawn in the inspector
- `string _DisplayName`: The name of the custom editor, displayed in the Multiple Editors Manager window
- `string _Description`: A short description about what your editor does, displayed in the Multiple Editors Manager window
- `bool _RequiresConstantRepaint = false`: If true, the extended `Editor` will set `Editor.RequiresConstantRepaint()` to true

---

```cs
public virtual void OnEnable()
```

Called when the extended `Editor` instance is enabled.

```cs
public virtual void OnDisable()
```

Called when the extended `Editor` instance is disabled.

```cs
public virtual void OnBeforeHeaderGUI()
```

Called before the default extended `Editor`'s header is displayed.

```cs
public virtual void OnHeaderGUI()
```

Called after the default extended `Editor`'s header is displayed.

```cs
public virtual void OnBeforeInspectorGUI()
```

Called before the default extended `Editor`'s inspector is displayed.

```cs
public virtual void OnInspectorGUI()
```

Called after the default extended `Editor`'s inspector is displayed.

```cs
public virtual void OnSceneGUI()
```

Handles Scene view events. Note that this method is called only if the target type is a scene object (instances of `MonoBehaviour` for example).

---

```cs
protected void Repaint()
```

Alias of the extended `Editor`'s `Repaint()` method. Repaint the inspector that shows this editor.

---

```cs
public TTarget Target { get; }
public TTarget target { get; }
```

Gets the object being inspected.

---

```cs
public TTarget[] Targets { get; }
public TTarget[] targets { get; }
```

Gets an array of all the objects being inspected.

---

```cs
public SerializedObject SerializedObject { get; }
public SerializedObject serializedObject { get; }
```

Aliases of the extended `Editor`'s `serializeObject` accessor. A `SerializedObject` representing the object or objects being inspected.