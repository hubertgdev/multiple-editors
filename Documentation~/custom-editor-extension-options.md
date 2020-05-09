# Muffin Dev for Unity - Multiple Editors Manager - `CustomEditorExtensionOptions`

Represents the options and default values of a `CustomEditorExtension`.

## Constructor

```cs
public CustomEditorExtensionOptions(int _DefaultOrder = 0, string _DisplayName = null, string _Description = null, bool _RequiresConstantRepaint = false)
```

Creates a CustomEditorExtensionOptions instance.

- `int _DefaultOrder = 0`: The defaut position of the `CustomEditorExtension` in the open Editor where it's used. The higher the value, the higher the custom editor will appear in the inspector
- `string _DisplayName = null`: The name that will be displayed in the Multiple Editors Manager window, in the editor. If null or empty, the window uses the class name of the `CustomEditorExtension` instead
- `string _Description = null`: A short description about what the custom editor does
- `bool _RequiresConstantRepaint = false`: If true, the Editor instance that uses this `CustomEditorExtension` will set `Editor.RequiresConstantRepaint()` to `true`, so the inspector view will be drawn even if it's not focused.

## Properties

```cs
public static readonly CustomEditorExtensionOptions Default = new CustomEditorExtensionOptions();
```

Returns a new `CustomEditorExtensionOptions` instance with default values.

---

```cs
public string displayName;
```

The name that will be displayed in the Multiple Editors Manager window, in the editor. If null or empty, the window uses the class name of the `CustomEditorExtension` instead.

---

```cs
public string description;
```

A short description about what the custom editor does.

---

```cs
public string defaultOrder;
```

The defaut position of the `CustomEditorExtension` in the open Editor where it's used. The higher the value, the higher the custom editor will appear in the inspector.

---

```cs
public string requiresConstantRepaint;
```

If true, the Editor instance that uses this `CustomEditorExtension` will set `Editor.RequiresConstantRepaint()` to `true`, so the inspector view will be drawn even if it's not focused.