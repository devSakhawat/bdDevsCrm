# Kendo UI 2024 Q4 Files

## Required Files

Please manually add the following Kendo UI 2024 Q4 files to this directory:

### JavaScript Files (js/)
- `kendo.all.min.js` - Complete Kendo UI library

### CSS Files (css/)
- `kendo.common.min.css` - Common Kendo UI styles
- `kendo.default.min.css` - Default theme styles

### Styles (styles/)
- Theme-specific resources (fonts, images, etc.)

## How to Obtain Kendo UI Files

1. Download Kendo UI from Telerik (requires license): https://www.telerik.com/kendo-ui
2. Extract the downloaded package
3. Copy the required files from the package to the appropriate folders above
4. Verify files are loaded correctly in the browser console

## File Structure
```
kendo_2024_q4/
├── js/
│   └── kendo.all.min.js
├── css/
│   ├── kendo.common.min.css
│   └── kendo.default.min.css
└── styles/
    └── (theme resources)
```

## Notes
- Kendo UI requires a commercial license
- Files are NOT included in version control due to licensing
- After adding files, uncomment Kendo UI references in `_Layout.cshtml`
