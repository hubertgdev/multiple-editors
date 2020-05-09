# Muffin Dev for Unity - Multiple Editors Manager - `MultipleEditorsHandler`

Base class for creating a custom editor view that can display multiple different editor extensions.

## Usage

First this example, create a custom `MonoBehaviour`:

```cs
using UnityEngine;

public class MyBehaviour : MonoBehaviour { }
```

Then, create the multiple editors handler for that specific type.

```cs
using UnityEngine;
using UnityEditor;
using MuffinDev.EditorUtils.MultipleEditors;

[CustomEditor(typeof(MyBehaviour))]
[CanEditMultipleObjects]
public class MyBehaviourMultipleEditorsHandler : MultipleEditorsHandler<MyBehaviour> { }
```

You must use `CustomEditor` attribute to tell Unity that this class is a custom editor of the target type (here `MyBehaviour`). The `CanEditMultipleObject` attribute is optional, but is needed if you want your custom editor extensions to deal with multiple selected objects.

Inheriting from `MultipleEditorsHandler` makes this class a user of the `MultipleEditorsManager`, so you can create custom extensions that target the type you specify (here `MyBehaviour`).

Note that you don't need any content in that class: it already works! Create your custom editor extensions by using `CustomEditorExtension` class. See the example above, or the [`CustomEditorExtension` class documentation](./custom-editor-extension.md) for more informations about creating extensions.

Note that native objects (like `GameObject` or `Transform`) can have a special behavior that prevents us to manipulate them as we can do for our custom types. See [`NativeObjectMultipleEditorsHandler`] for more informations about creating multiple editors handlers for native objects.

## Methods

```cs
protected virtual void OnEnable()
```

This function is called when the object is loaded. Loads the custom editors extensions, and enable them.

---

```cs
protected virtual void OnDisable()
```

This function is called when the scriptable object goes out of scope. Disables the loaded custom editor extensions.

---

```cs
protected override void OnHeaderGUI()
```

Called when the header of the object being inspected is drawn. By default, calls `OnBeforeHeaderGUI()` on loaded custom editor extensions, draws the original header, then call `OnHeaderGUI()` on loaded custom editor extensions.

---

```cs
public override void OnInspectorGUI()
```

Called when the inspector of the object being inspected is drawn. By default, calls `OnBeforeInspectorGUI()` on loaded custom editor extensions, draws the original inspector, then call `OnInspectorGUI()` on loaded custom editor extensions.

---

```cs
protected void OnSceneGUI()
```

Handles scene events. Note that this message is sent by Unity only when inspecting scene objects.

---

```cs
public override bool RequiresConstantRepaint()
```

Defines if this Editor should be repainted constantly, (similar to an Update() for an Editor class).

---

```cs
protected void LoadExtensions()
```

Creates all the `CustomEditorExtension` that have the same target type as this `Editor`.

---

```cs
protected void EnableCustomEditors()
```

Calls `OnEnable()` on each loaded custom editor extensions.

---

```cs
protected void DisableCustomEditors()
```

Calls `OnDisable()` on each loaded custom editor extensions.

---

```cs
protected void DrawCustomEditorsBeforeHeaderGUI()
```

Calls `OnBeforeHeaderGUI()` on each loaded custom editor extensions.

---

```cs
protected void DrawCustomEditorsHeaderGUI()
```

Calls `OnHeaderGUI()` on each loaded custom editor extensions.

---

```cs
protected void DrawCustomEditorsBeforeInspectorGUI()
```

Calls `OnBeforeInspectorGUI()` on each loaded custom editor extensions.

---

```cs
protected void DrawCustomEditorsInspectorGUI()
```

Calls `OnInspectorGUI()` on each loaded custom editor extensions.

---

```cs
protected void DrawCustomEditorsSceneGUI()
```

Calls `OnSceneGUI()` on each loaded custom editor extensions.