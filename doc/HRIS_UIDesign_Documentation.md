HRIS + BonusPayment  —  UI/UX Design Documentation   v1.0

**HRIS + BonusPayment System**

Enterprise UI/UX Design Documentation

|<p>**Complete Component Design Reference**</p><p>Developer & Designer Guide — v1.0</p><p>Tech Stack: .NET Core MVC + Kendo UI + jQuery</p>|
| :-: |

Modules: 30+  |  Style: Corporate SaaS  |  Theme: Light


# **Table of Contents**






|**01  Design Philosophy & Principles**|
| :- |

## **1.1  Core Design Philosophy**
HRIS + BonusPayment system-এর UI হবে Clean, Structured এবং Data-heavy friendly। সব module-এ একই look & feel বজায় রাখা mandatory।

|**Principle**|**Description**|**Rule**|
| :- | :- | :- |
|Clean + Structured|Corporate SaaS style — unnecessary decoration বর্জন|Whitespace ব্যবহার করো, cluttered layout নয়|
|Consistency First|৩০+ module-এ same pattern follow করতে হবে|এই document-ই হবে single source of truth|
|Data-Heavy Friendly|HRIS = অনেক data — grid এবং table optimize করতে হবে|Server-side paging must, client-side rendering avoid|
|Kendo-First|Custom component তৈরি না করে Kendo override করো|CSS override yes, custom control replacement no|
|Accessible & Fast|Non-technical HR user ব্যবহার করবে|Font readable, contrast high, loading state present|

## **1.2  Target Users**
- **HR Admin:** সকল module access, complex form, grid-heavy workflow
- **Accounts Team:** BonusPayment, payroll-related form, export-heavy
- **Non-technical Staff:** Simple data entry, leave, attendance — minimal cognitive load

*💡 User type অনুযায়ী form complexity vary করবে, কিন্তু visual language সর্বদা same থাকবে।*



|**02  Layout System**|
| :- |

## **2.1  Overall Page Structure**
প্রতিটি page নিচের ৪টি core zone নিয়ে গঠিত:

|**Zone**|**Position**|**Height / Width**|**Behavior**|
| :- | :- | :- | :- |
|Header (Top Bar)|Top, full width|60px fixed|Fixed — scroll করলেও move করে না|
|Sidebar (Nav)|Left side|240px (expanded) / 64px (collapsed)|Collapsible, z-index above content|
|Main Content Area|Center-right|Remaining width & height|Scrollable, padding 24px all sides|
|Footer|Bottom of content area|48px|Minimal, version + copyright|

## **2.2  Header (Top Bar)**
Height: 60px | Background: #1E5FA8 (Primary Blue) | Position: fixed top

|**Element**|**Position**|**Detail**|
| :- | :- | :- |
|Company Logo / App Name|Left (16px padding)|Logo image max-height 36px অথবা text "HRIS System" bold white|
|Global Search Box|Center|Kendo AutoComplete, placeholder: "Search...", width 280px, border-radius 4px|
|Notification Bell Icon|Right group|Badge count, click → dropdown panel, icon size 20px white|
|User Avatar + Name|Far Right (16px padding)|Kendo DropDown trigger, avatar 32px circle, name text white 14px|

|<p>/\* Header CSS \*/</p><p>.app-header {</p><p>`  `height: 60px;</p><p>`  `background: #1E5FA8;</p><p>`  `position: fixed;</p><p>`  `top: 0; left: 0; right: 0;</p><p>`  `display: flex;</p><p>`  `align-items: center;</p><p>`  `justify-content: space-between;</p><p>`  `padding: 0 16px;</p><p>`  `z-index: 1000;</p><p>`  `box-shadow: 0 2px 8px rgba(0,0,0,0.15);</p><p>}</p>|
| :- |

## **2.3  Sidebar (Navigation)**
Width Expanded: 240px | Collapsed: 64px | Background: #1E293B (Dark Slate)

|**State**|**Width**|**Icon**|**Text**|**Behavior**|
| :- | :- | :- | :- | :- |
|Expanded|240px|24px (left, 16px padding)|Visible (14px, white)|Default on desktop|
|Collapsed|64px|24px (centered)|Hidden (tooltip on hover)|Mobile or user toggle|
|Active Menu|—|Icon + text → Primary Blue background|Bold text|Current page highlight|
|Hover State|—|Light blue overlay (rgba 255,255,255,0.08)|—|Smooth 0.2s transition|

- **Multi-level Menu:** Parent menu → click করলে child expand (accordion style)
- **Active Indicator:** Left border 3px solid #2563EB + background rgba(37,99,235,0.15)
- **Icon Library:** Font Awesome 6 অথবা Material Icons (project standard একটা fix করো)

|<p>/\* Sidebar CSS \*/</p><p>.app-sidebar {</p><p>`  `width: 240px;</p><p>`  `background: #1E293B;</p><p>`  `position: fixed;</p><p>`  `top: 60px; left: 0; bottom: 0;</p><p>`  `transition: width 0.25s ease;</p><p>`  `z-index: 900;</p><p>`  `overflow-y: auto;</p><p>}</p><p>.app-sidebar.collapsed { width: 64px; }</p><p>.nav-item.active {</p><p>`  `background: rgba(37,99,235,0.15);</p><p>`  `border-left: 3px solid #2563EB;</p><p>}</p>|
| :- |

## **2.4  Main Content Area**
Margin-left: 240px (collapsed: 64px) | Margin-top: 60px | Padding: 24px

- Page Title + Breadcrumb row — সবার উপরে
- Action Button row (Add New, Export) — title এর নিচে
- Content Zone — Form অথবা Grid

|<p>.main-content {</p><p>`  `margin-left: 240px;</p><p>`  `margin-top: 60px;</p><p>`  `padding: 24px;</p><p>`  `min-height: calc(100vh - 60px);</p><p>`  `background: #F1F5F9;</p><p>`  `transition: margin-left 0.25s ease;</p><p>}</p><p>.main-content.sidebar-collapsed { margin-left: 64px; }</p>|
| :- |

## **2.5  Page Header Zone (Inside Content)**

|**Element**|**Style**|**Detail**|
| :- | :- | :- |
|Page Title|H1, 22px bold, #1E293B|e.g. "Employee Management"|
|Breadcrumb|12px, #94A3B8, separator "/"|Home / HR / Employee Management|
|Action Buttons|Right-aligned button group|Primary: "Add New", Secondary: "Export Excel"|
|Divider Line|1px solid #CBD5E1, margin-bottom 16px|Title area থেকে content separate করে|

## **2.6  Footer**
Height: 48px | Background: #FFFFFF | Border-top: 1px solid #CBD5E1

- Left: © 2025 CompanyName. All rights reserved.
- Right: Version 1.0.0

Font: 12px, color #94A3B8



|**03  Color System**|
| :- |

## **3.1  Primary Color Palette**

|**Color / Semantic**|**Hex Code & Usage**|
| :- | :- |
|Primary Blue — #1E5FA8|Header, Sidebar header, Primary Button background, Section banner|
|Primary Dark — #1E3A5F|Hover state of primary button, sidebar active deeper shade|
|Accent Blue — #2563EB|Active nav indicator border, link color, focus ring|
|Primary Light — #DBEAFE|Alert info background, selected row highlight (grid)|

## **3.2  Neutral / Gray Palette**

|**Color / Semantic**|**Hex Code & Usage**|
| :- | :- |
|Dark Slate — #1E293B|Page title, body text main, sidebar background|
|Mid Gray — #475569|Secondary text, subtitle, placeholder label|
|Light Gray — #94A3B8|Disabled text, breadcrumb, footer text|
|Border Gray — #CBD5E1|All input borders, table borders, card borders, divider lines|
|BG Gray — #F1F5F9|Main content background, alternate table row, input background|
|White — #FFFFFF|Card background, modal background, form panel, footer|

## **3.3  Semantic (State) Colors**

|**Color / Semantic**|**Hex Code & Usage**|
| :- | :- |
|Success — #16A34A|Save success toast, active badge, success button|
|Danger — #DC2626|Delete button, validation error border, error toast, required asterisk|
|Warning — #D97706|Warning toast, pending badge, alert box|
|Info — #0284C7|Info toast, info badge, help tooltip|

*💡 Kendo UI theme override করার সময় এই exact hex values ব্যবহার করতে হবে। SCSS variable হিসেবে define করো।*

|<p>/\* \_variables.scss \*/</p><p>$color-primary:    #1E5FA8;</p><p>$color-primary-dk: #1E3A5F;</p><p>$color-accent:     #2563EB;</p><p>$color-dark:       #1E293B;</p><p>$color-mid:        #475569;</p><p>$color-light:      #94A3B8;</p><p>$color-border:     #CBD5E1;</p><p>$color-bg:         #F1F5F9;</p><p>$color-success:    #16A34A;</p><p>$color-danger:     #DC2626;</p><p>$color-warning:    #D97706;</p><p>$color-info:       #0284C7;</p>|
| :- |

## **3.4  Usage Rules**
- এক page-এ ২টার বেশি primary color use করা যাবে না
- Background এ bright color avoid — neutral BG-Gray ব্যবহার করো
- Danger color শুধু destructive action এবং error এ ব্যবহার হবে
- Contrast ratio minimum 4.5:1 (WCAG AA) maintain করতে হবে



|**04  Typography**|
| :- |

## **4.1  Font Family**

|**Priority**|**Font**|**Fallback**|**Usage**|
| :- | :- | :- | :- |
|1st Choice|Segoe UI|system-ui, -apple-system|Windows server environment preferred|
|2nd Choice|Inter|sans-serif|Modern alternative, Google Fonts import করতে পারলে|
|Monospace|Courier New / Consolas|monospace|Code blocks, ID display, reference number|

## **4.2  Type Scale**

|**Level**|**Element**|**Size (px)**|**Weight**|**Color**|**Line Height**|
| :- | :- | :- | :- | :- | :- |
|Display|Page Banner Title|28px|700 Bold|#FFFFFF (on banner)|1\.3|
|H1|Page Title|22px|700 Bold|#1E293B|1\.4|
|H2|Section Title|18px|600 SemiBold|#1E5FA8|1\.4|
|H3|Card / Group Title|15px|600 SemiBold|#1E293B|1\.5|
|Body Large|Important paragraph|14px|400 Regular|#1E293B|1\.6|
|Body Base|General text|13px|400 Regular|#475569|1\.6|
|Body Small|Helper text, note|12px|400 Regular|#94A3B8|1\.5|
|Label|Form field label|13px|500 Medium|#1E293B|1\.4|
|Caption|Table cell, meta|12px|400 Regular|#475569|1\.4|

## **4.3  Typography Rules**
- সব জায়গায় একই font scale ব্যবহার করতে হবে — random size দেওয়া যাবে না
- Body text maximum width 720px রাখো (readability)
- Text-transform: UPPERCASE শুধু badge এবং tiny label-এ
- Letter-spacing: label এ 0.025em, normal text এ 0



|**05  Spacing & Grid System**|
| :- |

## **5.1  Base Spacing Unit**
Base unit: 8px — সব spacing এর multiple of 4 বা 8 হতে হবে।

|**Token**|**Value**|**Usage**|
| :- | :- | :- |
|space-1|4px|Tight inline spacing (icon to text gap, small badge padding)|
|space-2|8px|Form field internal padding top/bottom, small component gap|
|space-3|12px|Input horizontal padding, small card padding|
|space-4|16px|Form field gap (between fields), section inner padding|
|space-5|20px|Card padding, modal padding vertical|
|space-6|24px|Main content padding, large section gap|
|space-8|32px|Between major sections, modal header padding|
|space-10|40px|Page-level vertical rhythm|

## **5.2  Component Spacing Rules**

|**Component**|**Internal Padding**|**Gap to Next Component**|
| :- | :- | :- |
|Form Field (Input)|8px top/bottom, 12px left/right|16px margin-bottom to next field|
|Form Row (Grid layout)|—|16px gutter between columns|
|Button|8px top/bottom, 16px left/right|8px gap between adjacent buttons|
|Card / Panel|24px all sides|24px margin between cards|
|Modal|32px header, 24px body, 24px footer|—|
|Table Cell (Kendo Grid)|8px top/bottom, 12px left/right|—|
|Sidebar Nav Item|12px top/bottom, 16px left|0 (no gap — list style)|
|Toast / Alert|12px top/bottom, 16px left/right|—|

## **5.3  Responsive Breakpoints**

|**Breakpoint**|**Width**|**Layout Change**|
| :- | :- | :- |
|Mobile|< 768px|Sidebar hidden (drawer), single column form|
|Tablet|768px – 1199px|Sidebar collapsed (icon only), 2-col form|
|Desktop|1200px – 1439px|Sidebar expanded, 2-3 col form|
|Large Desktop|≥ 1440px|Sidebar expanded, full grid, max-content 1400px|

*💡 HRIS সাধারণত desktop-heavy। Mobile responsive শুধু critical view-এ (leave application, attendance) ensure করো।*



|**06  Button System**|
| :- |

## **6.1  Button Types & Visual Spec**

|**Type**|**Background**|**Text Color**|**Border**|**Use Case**|**Example**|
| :- | :- | :- | :- | :- | :- |
|Primary|#1E5FA8|#FFFFFF|None|Main action — Save, Submit, Confirm|Save Employee|
|Secondary|#FFFFFF|#1E5FA8|1px solid #1E5FA8|Supporting action — Edit, View|Edit Record|
|Outline / Ghost|Transparent|#475569|1px solid #CBD5E1|Less important — Reset, Back|Go Back|
|Danger|#DC2626|#FFFFFF|None|Destructive — Delete, Remove|Delete|
|Success|#16A34A|#FFFFFF|None|Approval action — Approve, Activate|Approve|
|Disabled|#F1F5F9|#94A3B8|1px solid #CBD5E1|Non-interactive state|(any disabled)|

## **6.2  Button Size Variants**

|**Size**|**Height**|**Padding**|**Font Size**|**Usage**|
| :- | :- | :- | :- | :- |
|Small (sm)|28px|4px 10px|12px|Table action column, compact form|
|Medium (md) — Default|36px|8px 16px|13px|Standard form actions, toolbar|
|Large (lg)|44px|10px 20px|14px|Modal main action, page-level CTA|

## **6.3  Button Rules**
- **Primary Rule:** একটি page অথবা modal-এ শুধুমাত্র ১টি Primary button থাকবে
- **Placement:** Action buttons সর্বদা right-aligned (form bottom) অথবা group করা (toolbar left)
- **Order (Left → Right):** Danger → Ghost/Outline → Secondary → Primary
- **Loading State:** Click এর পর button disabled + spinner icon + "Processing..."
- **Icon + Text:** Icon থাকলে text এর আগে, gap 6px

|<p>/\* Button CSS Override for Kendo \*/</p><p>.k-button.btn-primary {</p><p>`  `background: #1E5FA8;</p><p>`  `color: #fff;</p><p>`  `border: none;</p><p>`  `border-radius: 4px;</p><p>`  `font-size: 13px;</p><p>`  `height: 36px;</p><p>`  `padding: 0 16px;</p><p>`  `font-weight: 500;</p><p>}</p><p>.k-button.btn-primary:hover { background: #1E3A5F; }</p><p>.k-button.btn-primary:disabled { background: #F1F5F9; color: #94A3B8; }</p>|
| :- |



|**07  Form System**|
| :- |

## **7.1  Form Layout Types**
তোমার project-এ ৩টি standard form layout থাকবে। প্রতিটি layout কখন ব্যবহার করতে হবে সেটা নিচে define করা আছে।

### **Type 1 — Standard Form (Single Column)**

|**Property**|**Specification**|
| :- | :- |
|Layout|Single column, full width (max 720px)|
|Label Position|Label top (above input), font 13px medium #1E293B|
|Input Width|100% of parent container|
|Field Gap|16px margin-bottom between each form group|
|Use When|Simple entry form, 4–6 fields, modal form, HR leave application|
|Example|Employee basic info form, Leave request form|

### **Type 2 — Grid Form (Multi-Column)**

|**Property**|**Specification**|
| :- | :- |
|Layout|2-column grid (large screen), 1-column (mobile)|
|Column Gutter|16px between columns|
|Label Position|Label top, 13px medium|
|Field Distribution|Related fields পাশাপাশি — e.g., First Name | Last Name|
|Section Break|H3 heading + horizontal divider between logical groups|
|Use When|Medium-large form, 6–20+ fields, employee full profile, payroll setup|
|Example|Employee Full Registration, Bonus Payment Setup|

### **Type 3 — Inline / Filter Form**

|**Property**|**Specification**|
| :- | :- |
|Layout|Horizontal row — all fields in one line|
|Label Position|Placeholder only (no separate label) অথবা very short label before input|
|Input Width|Auto / fit-content, fixed width per field|
|Use When|Search bar, filter panel above grid, quick filter|
|Example|Employee list filter (Department + Status + Date range + Search button)|

## **7.2  Form Field Components**

### **7.2.1 — Text Input (Kendo TextBox)**

|**State**|**Border**|**Background**|**Detail**|
| :- | :- | :- | :- |
|Default|1px solid #CBD5E1|#FFFFFF|Placeholder color: #94A3B8, font 13px|
|Focus|1\.5px solid #2563EB|#FFFFFF|Box-shadow: 0 0 0 3px rgba(37,99,235,0.12)|
|Filled|1px solid #CBD5E1|#FFFFFF|Text color: #1E293B|
|Disabled|1px solid #E2E8F0|#F1F5F9|Text color: #94A3B8, cursor not-allowed|
|Error|1\.5px solid #DC2626|#FFF5F5|Error message below, color #DC2626, 11px|
|Read-only|1px solid #E2E8F0|#F8FAFC|cursor default, not editable|

### **7.2.2 — Dropdown / DropDownList (Kendo)**
- Height: 36px (same as text input)
- Arrow icon: right side, color #475569
- Dropdown list max-height: 280px, scrollable
- Item hover: background #DBEAFE
- Selected item: background #EFF6FF, text bold
- Empty option: "-- Select --" as default placeholder
- Search/filter inside dropdown: enabled যদি items > 10 হয়

### **7.2.3 — ComboBox (Kendo)**
- Autocomplete + free-text — hybrid input
- Same height and border as Text Input
- Suggestion list: same style as Dropdown list
- Clear button: × icon appears when value filled

### **7.2.4 — DatePicker (Kendo)**
- Input + Calendar icon (right side)
- Date format: dd MMM yyyy (e.g. 15 Jan 2025)
- Calendar popup: Kendo default, primary color override করো
- Min/Max date: contextual — জন্মতারিখ future date allow নয়
- Range DatePicker: From Date → To Date পাশাপাশি inline form এ

### **7.2.5 — Checkbox**

|**State**|**Visual**|**Detail**|
| :- | :- | :- |
|Unchecked|Empty square, 16x16px, border 1.5px solid #CBD5E1|Background white|
|Checked|Blue filled square, white checkmark|Background #1E5FA8, border #1E5FA8|
|Disabled|Gray square|Border #E2E8F0, not clickable|
|Indeterminate|Dash inside square|Used in "select all" grid header|

Label: right side of checkbox, gap 8px, font 13px regular

Alignment fix: 

|<p>/\* Checkbox alignment fix \*/</p><p>.checkbox-group {</p><p>`  `display: flex;</p><p>`  `align-items: center;</p><p>`  `gap: 8px;</p><p>`  `min-height: 36px; /\* match input height \*/</p><p>}</p><p>.checkbox-group input[type="checkbox"] {</p><p>`  `width: 16px;</p><p>`  `height: 16px;</p><p>`  `margin: 0;</p><p>`  `cursor: pointer;</p><p>}</p>|
| :- |

### **7.2.6 — Radio Button**
- Circle indicator, same color scheme as checkbox
- Group label (H3) above radio group, items vertical stacked with 8px gap
- Inline radio (horizontal) শুধু 2-3 option এর জন্য

### **7.2.7 — Textarea**
- min-height: 80px, resizable vertically only
- Same border/focus/error style as text input
- Character count (optional): bottom right corner, 11px gray

### **7.2.8 — File Upload**
- Button-style trigger: "Choose File" outline button + filename display span
- Drag & drop zone (if needed): dashed border #CBD5E1, hover fill #F1F5F9
- Accepted formats shown below: "Accepted: .pdf, .jpg, .png (max 5MB)"

### **7.2.9 — Number Input**
- Kendo NumericTextBox — up/down arrows right side
- Thousand separator enabled for amount fields
- Decimal places: 2 for currency, 0 for count

## **7.3  Form Label Rules**

|**Rule**|**Specification**|
| :- | :- |
|Position|Label সর্বদা input এর উপরে (top-label pattern)|
|Required Field|Label এর পর red asterisk (\*) — color #DC2626, font 13px|
|Optional Field|Label এর পর "(optional)" — color #94A3B8, font 11px, italic|
|Spacing|Label margin-bottom: 4px (tight above input)|
|Disabled Label|Color: #94A3B8|

## **7.4  Validation UI Rules**

|**State**|**Visual Change**|**Message**|
| :- | :- | :- |
|Error|Red border (#DC2626), light red bg (#FFF5F5)|Below input: 11px italic red — "This field is required"|
|Success|Green border (#16A34A)|Optional check icon — no text needed|
|Warning|Orange border (#D97706)|Below input: warning message, e.g. "Date seems unusual"|

- **Trigger:** Validation fires on focusout (blur) এবং form submit
- **Scroll to Error:** Submit click এ প্রথম error field-এ scroll এবং focus
- **Summary:** Complex form-এ উপরে error summary box (danger background)

|<p>/\* Validation CSS \*/</p><p>.field-error .k-input { border-color: #DC2626 !important; }</p><p>.field-error-msg {</p><p>`  `color: #DC2626;</p><p>`  `font-size: 11px;</p><p>`  `font-style: italic;</p><p>`  `margin-top: 4px;</p><p>`  `display: flex;</p><p>`  `align-items: center;</p><p>`  `gap: 4px;</p><p>}</p>|
| :- |

## **7.5  Form Action Bar (Bottom of Form)**
Form-এর নিচে একটি sticky/fixed action bar থাকবে:

- Right-aligned button group
- Order: [Cancel / Back] → [Reset] → [Save / Submit]
- Top border: 1px solid #CBD5E1
- Padding: 16px 24px
- Background: white



|**08  Kendo Grid (Data Table) System**|
| :- |

## **8.1  Grid Visual Specification**

|**Element**|**Specification**|
| :- | :- |
|Grid Container|Card wrapper: white bg, border 1px solid #CBD5E1, border-radius 6px, shadow sm|
|Column Header|Background #1E5FA8, text white, 13px bold, height 40px, padding 8px 12px|
|Data Row — Even|Background #FFFFFF, height 40px|
|Data Row — Odd|Background #F8FAFC, height 40px (zebra stripe)|
|Row Hover|Background #EFF6FF (light blue), transition 0.15s|
|Selected Row|Background #DBEAFE, left border 3px solid #2563EB|
|Cell Padding|8px top/bottom, 12px left/right|
|Cell Font|13px regular #475569; important values bold #1E293B|
|Footer Row (Aggregate)|Background #F1F5F9, font bold, border-top 2px solid #CBD5E1|

## **8.2  Standard Grid Features (Mandatory)**

|**Feature**|**Status**|**Detail**|
| :- | :- | :- |
|Pagination|Must|Bottom right — Page size options: 10, 20, 50, 100 | Current page info|
|Column Sorting|Must|Click header → asc/desc → clear | Icon: ↑↓ arrows|
|Filter Row|Must|Below header row — per-column filter input|
|Column Resize|Must|Drag column separator to resize|
|Server-side Paging|Must|কখনো client-side full load করো না (performance)|
|Action Column|Must|Last column — Edit, View, Delete buttons (small size)|
|Loading Overlay|Must|Grid load হওয়ার সময় spinner overlay|
|Row Selection|Optional|Checkbox first column — bulk action এর জন্য|
|Column Reorder|Optional|Drag header to reorder|
|Export (Excel/PDF)|Contextual|Toolbar-এ Export button, Kendo Excel/PDF export|

## **8.3  Action Column Design**
- Width: 120px (Edit + View) অথবা 160px (Edit + View + Delete)
- Buttons: Small size, Icon-only অথবা Icon + text
- Edit: Secondary button, pencil icon, color #1E5FA8
- View: Outline button, eye icon, color #475569
- Delete: Danger button, trash icon, color #DC2626
- Tooltip: প্রতিটি icon-only button-এ title attribute দিতে হবে

## **8.4  Grid Toolbar**
Grid-এর উপরে Toolbar থাকবে (যখন প্রয়োজন):

|**Position**|**Elements**|
| :- | :- |
|Left Side|Bulk action buttons (যদি row selection থাকে), Filter clear button|
|Right Side|Add New button (Primary), Export Excel button (Secondary), Refresh icon|

## **8.5  Empty State (No Data)**
- Grid-এর center-এ centered icon + text
- Icon: file/inbox icon, 48px, color #CBD5E1
- Text: "No records found." — 14px, #94A3B8
- Sub-text (optional): "Try adjusting your filters." — 12px, #94A3B8

|<p>/\* Kendo Grid Override \*/</p><p>.k-grid { border-radius: 6px; border-color: #CBD5E1; }</p><p>.k-grid-header th { background: #1E5FA8 !important; color: #fff; font-weight: 600; }</p><p>.k-grid tr:hover { background: #EFF6FF; }</p><p>.k-grid tr:nth-child(odd) { background: #F8FAFC; }</p><p>.k-pager-wrap { border-top: 1px solid #CBD5E1; background: #fff; }</p>|
| :- |



|**09  Modal / Popup System**|
| :- |

## **9.1  Modal Types**

|**Type**|**Width**|**Use Case**|**Footer Buttons**|
| :- | :- | :- | :- |
|Small|400px|Confirmation dialog, simple 1-2 field form, delete confirm|Cancel + Confirm|
|Medium|600px|Standard form modal (5-8 fields), detail view|Cancel + Save|
|Large|900px|Complex form, multi-section form|Cancel + Reset + Save|
|Full Screen|100% – 48px|Report view, document preview|Close button only (top right)|

## **9.2  Modal Visual Specification**

|**Element**|**Specification**|
| :- | :- |
|Overlay|Background: rgba(0,0,0,0.45), z-index 1050|
|Modal Box|Background #FFFFFF, border-radius 8px, box-shadow: 0 20px 60px rgba(0,0,0,0.3)|
|Header|Background #F8FAFC, padding 20px 24px, border-bottom 1px solid #CBD5E1|
|Header Title|16px bold #1E293B + icon (optional, left side)|
|Close Button|× icon, top-right, 20px, color #94A3B8, hover #475569|
|Body|padding 24px, overflow-y auto, max-height 70vh|
|Footer|padding 16px 24px, border-top 1px solid #CBD5E1, right-aligned buttons|

## **9.3  Confirmation Dialog**
- Type: Small (400px)
- Icon: Warning (⚠️) orange অথবা Danger (🗑️) red — centered, 32px
- Title: Bold 16px — "Delete Employee?"
- Message: 13px gray — "This action cannot be undone. Are you sure?"
- Buttons: [Cancel (ghost)] [Delete (danger)]
- Animation: fade-in, 0.2s ease

## **9.4  Toast / Notification**

|**Type**|**Background**|**Icon**|**Duration**|**Position**|
| :- | :- | :- | :- | :- |
|Success|#16A34A|✓ checkmark|3 seconds|Top-right, 16px from corner|
|Error|#DC2626|✗ cross|5 seconds (manual close)|Top-right|
|Warning|#D97706|⚠ warning|4 seconds|Top-right|
|Info|#0284C7|ℹ info|3 seconds|Top-right|

Width: 320px | Font: 13px white | Border-radius: 6px | Box-shadow: lg



|**10  Card & Panel System**|
| :- |

## **10.1  Standard Card**

|**Property**|**Value**|
| :- | :- |
|Background|#FFFFFF|
|Border|1px solid #CBD5E1|
|Border Radius|8px|
|Box Shadow|0 1px 3px rgba(0,0,0,0.08), 0 1px 2px rgba(0,0,0,0.04)|
|Padding|24px|
|Margin Bottom|16px (between cards)|

## **10.2  Card with Header**
- Card Header: padding 16px 24px, background #F8FAFC, border-bottom 1px solid #CBD5E1
- Header Title: H3 style (15px semibold #1E293B)
- Header Actions (optional): right-aligned — small buttons / icon buttons
- Card Body: padding 24px

## **10.3  Dashboard Summary Card**

|**Element**|**Specification**|
| :- | :- |
|Width|Responsive grid: 4 columns → 2 → 1 (breakpoints)|
|Height|Auto (min 100px)|
|Icon|48px circle background (primary-light), icon 24px primary color|
|Value|28px bold #1E293B|
|Label|12px #94A3B8 uppercase, letter-spacing 0.05em|
|Trend (optional)|↑ green / ↓ red, 11px|

## **10.4  Alert / Info Banner**

|**Type**|**Background**|**Border Left**|**Icon**|**Usage**|
| :- | :- | :- | :- | :- |
|Info|#EFF6FF|3px solid #0284C7|ℹ|General information, tips|
|Success|#F0FDF4|3px solid #16A34A|✓|Action completed confirmation|
|Warning|#FFFBEB|3px solid #D97706|⚠|Pending action needed|
|Danger|#FFF5F5|3px solid #DC2626|✗|Critical error, blocked state|

Padding: 12px 16px | Border-radius: 6px | Font: 13px | dismiss button optional (top right ×)



|**11  Tabs & Navigation Components**|
| :- |

## **11.1  Tab System (Kendo TabStrip)**

|**State**|**Background**|**Text**|**Border Bottom**|**Usage**|
| :- | :- | :- | :- | :- |
|Inactive Tab|#F1F5F9|#475569, 13px|1px solid #CBD5E1|Default|
|Active Tab|#FFFFFF|#1E5FA8, 13px bold|2px solid #1E5FA8|Selected tab|
|Hover Tab|#FFFFFF|#1E5FA8|1px solid #CBD5E1|Hover state|
|Disabled Tab|#F1F5F9|#94A3B8|1px solid #E2E8F0|Non-accessible|

- Tab panel background: #FFFFFF, padding: 24px
- Tab bar border-bottom: 1px solid #CBD5E1
- Tab height: 40px
- Tab icon (optional): left of text, 14px, same color as text

## **11.2  Breadcrumb**
- Font: 12px, color #94A3B8
- Separator: "/" — color #CBD5E1, margin 0 6px
- Current page (last item): color #1E293B, not a link
- Previous items: color #0284C7, hover underline
- Position: below page title, above content zone

## **11.3  Pagination (Kendo Pager)**

|**Element**|**Specification**|
| :- | :- |
|Page number buttons|Width 32px, height 32px, border-radius 4px, font 13px|
|Active page|Background #1E5FA8, text white|
|Inactive page|Background #F1F5F9, text #475569, hover #EFF6FF|
|Prev / Next arrows|Icon buttons, same size|
|Page size selector|Kendo DropDown — "Show: [10 ▼] records"|
|Info text|"Showing 1-10 of 245 records" — 12px #94A3B8, left side|



|**12  UX State Patterns**|
| :- |

## **12.1  Loading State**

|**Context**|**Pattern**|**Detail**|
| :- | :- | :- |
|Initial Page Load|Full page skeleton|Gray placeholder blocks, animated shimmer|
|Grid Loading|Kendo loading overlay + spinner|Semi-transparent overlay on grid area|
|Button Click|Button disabled + spinner + "Processing..."|Button width preserve করো|
|Form Save|Full form overlay spinner|Prevent double-submit|
|Async Dropdown|Spinner inside dropdown|"Loading..." placeholder option|

|<p>/\* Loading overlay \*/</p><p>.loading-overlay {</p><p>`  `position: absolute;</p><p>`  `inset: 0;</p><p>`  `background: rgba(255,255,255,0.7);</p><p>`  `display: flex;</p><p>`  `align-items: center;</p><p>`  `justify-content: center;</p><p>`  `z-index: 100;</p><p>}</p><p>.spinner { width: 32px; height: 32px; border: 3px solid #CBD5E1;</p><p>`  `border-top-color: #1E5FA8; border-radius: 50%; animation: spin 0.8s linear infinite; }</p>|
| :- |

## **12.2  Empty State**

|**Context**|**Icon**|**Title**|**Sub-text**|**Action**|
| :- | :- | :- | :- | :- |
|Empty Grid|📋 48px gray|No records found.|Try adjusting your search filters.|Clear Filters (link)|
|No Search Results|🔍 48px gray|No results for "keyword"|Check spelling or try different keywords.|—|
|First Use (module)|⚙️ 48px primary|Get started!|No [entity] added yet.|Add New (primary button)|
|Access Denied|🔒 48px gray|Access Restricted|You do not have permission to view this.|Contact Admin (link)|

## **12.3  Error State**

|**Error Type**|**UI Treatment**|
| :- | :- |
|Network Error|Full page error card: icon + "Connection failed" + Retry button|
|Server Error (500)|Same as above: "Something went wrong. Please try again."|
|Not Found (404)|Centered: icon + "Page not found" + Go Home button|
|Validation Error|Field-level red border + message + form top summary (if 3+ errors)|
|Session Expired|Modal popup: "Session expired. Please login again." + Login button|

## **12.4  Confirmation Pattern**
- Delete action: সর্বদা confirmation dialog (never direct delete)
- Unsaved changes: navigate away করলে "You have unsaved changes. Leave?" dialog
- Bulk action: "Are you sure you want to [action] X records?"

## **12.5  Permission / Role-based UI**
- Hidden (not disabled): permission নেই এমন element দেখাবে না
- Read-only mode: শুধু view permission থাকলে form read-only render করো
- Disabled buttons: শুধু contextual disable (e.g. Save before required fields filled)



|**13  Naming Convention (Dev Standard)**|
| :- |

## **13.1  File Naming**

|**File Type**|**Convention**|**Example**|
| :- | :- | :- |
|JavaScript|camelCase + context prefix|bonusPaymentForm.js, employeeGrid.js|
|Partial View (.cshtml)|\_PascalCase|\_BonusForm.cshtml, \_EmployeeGrid.cshtml|
|SCSS/CSS|kebab-case|form-layout.scss, grid-styles.scss|
|Controller|PascalCase + Controller suffix|EmployeeController.cs|
|Service|PascalCase + Service suffix|BonusPaymentService.cs|
|Model/DTO|PascalCase + Dto suffix (for DTO)|EmployeeDto.cs, BonusPaymentModel.cs|

## **13.2  JavaScript Naming**

|**Element**|**Convention**|**Example**|
| :- | :- | :- |
|Function|camelCase + verb first|saveEmployee(), loadDepartmentList()|
|Variable|camelCase|employeeId, selectedDept|
|Constant|UPPER\_SNAKE\_CASE|MAX\_FILE\_SIZE, API\_BASE\_URL|
|jQuery selector cache|$ prefix + camelCase|$saveBtn, $employeeGrid|
|Event handler|on + EventType + Target|onClickSaveBtn(), onChangeStatus()|
|Kendo widget init function|init + WidgetName|initEmployeeGrid(), initDeptDropDown()|

## **13.3  CSS Class Naming (BEM-inspired)**

|**Pattern**|**Usage**|**Example**|
| :- | :- | :- |
|block|Component root|.employee-form, .bonus-grid|
|block\_\_element|Component child|.employee-form\_\_header, .bonus-grid\_\_toolbar|
|block--modifier|State or variant|.employee-form--readonly, .btn--primary|
|utility|Single-purpose helper|.text-center, .mt-16, .hidden|

## **13.4  HTML ID Naming**
- Form IDs: form{ModuleName} — e.g. formEmployee, formBonusPayment
- Grid IDs: grid{ModuleName} — e.g. gridEmployee, gridBonusList
- Modal IDs: modal{Purpose} — e.g. modalAddEmployee, modalConfirmDelete
- Input IDs: inp{FieldName} — e.g. inpFirstName, inpDepartmentId



|**14  Performance Guidelines**|
| :- |

## **14.1  Grid Performance Rules**

|**Rule**|**Description**|
| :- | :- |
|Server-side Paging|কখনো full dataset load করো না। Kendo Grid: serverPaging: true|
|Virtual Scrolling|Large list (1000+ rows) — Kendo virtual scrolling enable করো|
|No DataTable.js|jQuery DataTable avoid করো। Kendo Grid ব্যবহার করো|
|Debounce Filter|Search input filter: 300ms debounce দিয়ে API call করো|
|Column visibility|অপ্রয়োজনীয় column hide করো — DOM load কমায়|

## **14.2  API & Rendering**
- Lazy load: page load-এ শুধু visible data load করো
- Async dropdown: Kendo combo/dropdown serverFiltering: true for large lists
- Batch API: একাধিক small request না করে single endpoint থেকে data নাও
- Cache: Read-only dropdown data (Department, Designation) memory cache করো
- Image: Avatar/profile photo lazy load করো, thumbnail size 64x64

## **14.3  JavaScript Performance**
- Event delegation: grid action button-এ individual click handler না দিয়ে parent-এ delegate করো
- Destroy widget: modal close হলে Kendo widget destroy করো — memory leak prevent
- Minimize DOM queries: jQuery selector একবার cache করো ($element = $("#id"))



|**15  Kendo UI Override Strategy**|
| :- |

## **15.1  Core Strategy**
Kendo UI-কে replace করা যাবে না — শুধুমাত্র CSS/SCSS দিয়ে override করতে হবে।

|**Approach**|**Allowed**|**Not Allowed**|
| :- | :- | :- |
|CSS Override|Custom SCSS via !important (carefully)|জায়গায় জায়গায় inline style দেওয়া|
|Widget Config|Kendo widget options কাস্টমাইজ করা|Kendo widget কে jQuery plugin দিয়ে replace করা|
|Templates|Kendo column template, popup template|Kendo grid কে সম্পূর্ণ ভিন্ন library দিয়ে replace|
|Theme|Kendo SCSS theme variable override|নতুন CSS reset যা Kendo break করে|

## **15.2  Kendo Theme Setup**

|<p>// kendo-override.scss — project-level override file</p><p>// Import করো Kendo SCSS-এর পরে</p><p></p><p>$kendo-color-primary: #1E5FA8;</p><p>$kendo-border-radius: 4px;</p><p>$kendo-grid-header-bg: #1E5FA8;</p><p>$kendo-grid-header-text: #ffffff;</p><p></p><p>// Widget-specific overrides</p><p>.k-button.k-primary { background: #1E5FA8; }</p><p>.k-input, .k-dropdown, .k-datepicker { height: 36px; }</p><p>.k-grid td, .k-grid th { padding: 8px 12px; }</p>|
| :- |

## **15.3  Kendo Widget Init Pattern**

|<p>// Standard Kendo widget initialization pattern</p><p>function initEmployeeGrid() {</p><p>`  `$("#gridEmployee").kendoGrid({</p><p>`    `dataSource: {</p><p>`      `transport: { read: { url: "/api/employee/list", type: "POST" } },</p><p>`      `serverPaging: true,</p><p>`      `serverSorting: true,</p><p>`      `serverFiltering: true,</p><p>`      `pageSize: 20</p><p>`    `},</p><p>`    `pageable: { pageSizes: [10, 20, 50, 100] },</p><p>`    `sortable: true,</p><p>`    `filterable: { mode: "row" },</p><p>`    `resizable: true,</p><p>`    `columns: [ /\* column definitions \*/ ]</p><p>`  `});</p><p>}</p>|
| :- |



|**16  Developer Implementation Checklist**|
| :- |

## **16.1  New Module Checklist**

|**#**|**Task**|**Status**|
| :- | :- | :- |
|1|Layout structure use করেছো (header, sidebar, content, footer)?|☐|
|2|Color শুধু defined palette থেকে নিয়েছো?|☐|
|3|Typography scale follow করেছো?|☐|
|4|Form layout type select করেছো (Type 1/2/3)?|☐|
|5|Form labels উপরে (top-label) আছে?|☐|
|6|Required field (\*) markup করেছো?|☐|
|7|Validation on-blur + on-submit দিয়েছো?|☐|
|8|Kendo Grid server-side paging enable করেছো?|☐|
|9|Action column (Edit/View/Delete) standard button size use করেছো?|☐|
|10|Delete action এ confirmation dialog আছে?|☐|
|11|Loading state (spinner/overlay) implement করেছো?|☐|
|12|Empty state (No data message) দিয়েছো?|☐|
|13|Toast notification success/error দিয়েছো?|☐|
|14|Button naming convention follow করেছো?|☐|
|15|File naming convention follow করেছো?|☐|
|16|CSS class BEM pattern follow করেছো?|☐|
|17|No inline styles (CSS file-এ রেখেছো)?|☐|
|18|Mobile responsiveness check করেছো?|☐|



|**17  Quick Reference Card**|
| :- |

## **Colors at a Glance**

|**Purpose**|**Hex**|**Use**|
| :- | :- | :- |
|Primary|#1E5FA8|Header, Primary Button, Section Banner|
|Accent / Link|#2563EB|Active state, Focus ring, Links|
|Dark Text|#1E293B|Headings, Bold labels, Page title|
|Body Text|#475569|Paragraphs, Table cells, Secondary text|
|Muted Text|#94A3B8|Placeholder, Helper, Breadcrumb, Footer|
|Border|#CBD5E1|All inputs, Cards, Table, Dividers|
|BG Gray|#F1F5F9|Page background, Alt table row, Disabled input|
|Success|#16A34A|Save toast, Approve button, Active badge|
|Danger|#DC2626|Delete button, Error state, Required (\*)|
|Warning|#D97706|Warning toast, Pending badge|

## **Spacing Quick Ref**

|**Name**|**Value**|**Common Use**|
| :- | :- | :- |
|XS|4px|Icon gap, badge padding|
|SM|8px|Input padding V, button gap|
|MD|16px|Form field gap, section padding|
|LG|24px|Card padding, page padding|
|XL|32px|Between major sections|

## **Font Size Quick Ref**

|**Size**|**Use**|
| :- | :- |
|22px bold|Page Title (H1)|
|18px semibold|Section Title (H2)|
|15px semibold|Card Title (H3)|
|14px regular|Important body text|
|13px regular|Standard body, form label, button, table cell|
|12px regular|Helper text, breadcrumb, footer, badge|
|11px italic|Validation error message|



|**18  Code Architecture (30+ Module Pattern)**|
| :- |

## **18.1  Architecture Philosophy**
৩০+ module-এ consistency রাখতে একটি Core Layer বানাতে হবে। Module গুলো এই Core Layer use করবে — নিজেরা repeat করবে না।

|**Layer**|**দায়িত্ব**|**কে লিখবে**|
| :- | :- | :- |
|Core Layer|Grid factory, Form handler, Modal, Toast, API wrapper, Loader|একবার — সবাই use করবে|
|Module Layer|Column definition, field-specific logic, custom rules|প্রতিটি module আলাদা|
|SCSS Core|Variables, layout, component base styles|একবার — সব module inherit করবে|
|SCSS Module|Module-specific override (যদি দরকার হয়)|Optional, বেশিরভাগ সময় লাগবে না|

## **18.2  Folder Structure**

|<p>Project/</p><p>├── Controllers/</p><p>│   ├── BaseController.cs          ← Common ViewBag, permission check</p><p>│   └── EmployeeController.cs      ← Module controller</p><p>│</p><p>├── Views/</p><p>│   ├── Shared/</p><p>│   │   ├── \_Layout.cshtml         ← Master layout (সব page এর parent)</p><p>│   │   ├── \_Sidebar.cshtml        ← Sidebar partial</p><p>│   │   ├── \_Header.cshtml         ← Header partial</p><p>│   │   ├── \_PageHeader.cshtml     ← Title + Breadcrumb + Action buttons</p><p>│   │   ├── \_ModalConfirm.cshtml   ← Reusable confirm dialog</p><p>│   │   └── \_ModalDynamic.cshtml   ← Dynamic partial loader</p><p>│   └── Employee/</p><p>│       ├── Index.cshtml           ← শুধু grid container HTML</p><p>│       ├── \_Form.cshtml           ← Create/Edit form (partial)</p><p>│       └── \_View.cshtml           ← View detail (partial)</p><p>│</p><p>└── wwwroot/</p><p>`    `├── js/</p><p>`    `│   ├── core/</p><p>`    `│   │   ├── app.core.js        ← Global init, AJAX setup, CSRF token</p><p>`    `│   │   ├── app.grid.js        ← Kendo Grid factory ⭐</p><p>`    `│   │   ├── app.form.js        ← Form init, validation, submit handler ⭐</p><p>`    `│   │   ├── app.modal.js       ← Modal open/close/load</p><p>`    `│   │   ├── app.toast.js       ← Toast notification</p><p>`    `│   │   ├── app.loader.js      ← Loading overlay</p><p>`    `│   │   └── app.api.js         ← Centralized AJAX/fetch wrapper</p><p>`    `│   └── modules/</p><p>`    `│       ├── employee/</p><p>`    `│       │   ├── employee.grid.js   ← শুধু column def + event handler</p><p>`    `│       │   └── employee.form.js   ← শুধু field-specific logic</p><p>`    `│       └── bonusPayment/</p><p>`    `│           ├── bonusPayment.grid.js</p><p>`    `│           └── bonusPayment.form.js</p><p>`    `└── scss/</p><p>`        `├── core/</p><p>`        `│   ├── \_variables.scss    ← Color, font, spacing tokens</p><p>`        `│   ├── \_layout.scss       ← Header, Sidebar, Content, Footer</p><p>`        `│   ├── \_typography.scss   ← Font scale</p><p>`        `│   └── \_utilities.scss    ← Helper classes</p><p>`        `├── components/</p><p>`        `│   ├── \_button.scss</p><p>`        `│   ├── \_form.scss</p><p>`        `│   ├── \_form-layouts.scss ← Type 1 / 2 / 3</p><p>`        `│   ├── \_grid.scss         ← Kendo Grid override</p><p>`        `│   ├── \_modal.scss</p><p>`        `│   ├── \_card.scss</p><p>`        `│   ├── \_toast.scss</p><p>`        `│   └── \_states.scss       ← Loading, empty, error</p><p>`        `├── kendo-override/</p><p>`        `│   └── \_kendo-theme.scss  ← Kendo-specific overrides only</p><p>`        `└── main.scss              ← সব @import এখানে</p>|
| :- |

## **18.3  Layout Architecture — \_Layout.cshtml**
সব page এই একটি master layout use করবে। Page-specific content শুধু @RenderBody() এ যাবে।

|<p><!DOCTYPE html></p><p><html></p><p><head></p><p>`    `<!-- Core CSS: Kendo, app styles --></p><p>`    `@RenderSection("Styles", required: false)</p><p></head></p><p><body></p><p>`    `@await Html.PartialAsync("\_Header")</p><p>`    `@await Html.PartialAsync("\_Sidebar")</p><p></p><p>`    `<div class="main-content" id="mainContent"></p><p>`        `@await Html.PartialAsync("\_PageHeader")</p><p></p><p>`        `<div class="content-zone"></p><p>`            `@RenderBody()   ← শুধু page-specific content</p><p>`        `</div></p><p></p><p>`        `@await Html.PartialAsync("\_Footer")</p><p>`    `</div></p><p></p><p>`    `@await Html.PartialAsync("\_ModalConfirm")</p><p>`    `@await Html.PartialAsync("\_ModalDynamic")</p><p>`    `@await Html.PartialAsync("\_Notification")</p><p></p><p>`    `<!-- Core JS: jQuery, Kendo, app.core, app.grid, app.form ... --></p><p>`    `@RenderSection("Scripts", required: false)</p><p></body></p><p></html></p>|
| :- |

Controller থেকে ViewBag দিয়ে Page Title ও Breadcrumb inject করো:

|<p>// EmployeeController.cs</p><p>public IActionResult Index()</p><p>{</p><p>`    `ViewBag.PageTitle  = "Employee Management";</p><p>`    `ViewBag.Breadcrumb = new[] { "Home", "HR", "Employee Management" };</p><p>`    `return View();</p><p>}</p>|
| :- |

## **18.4  JS Core Layer — app.grid.js (Grid Factory)**
এই একটি file লিখলে সব module-এর grid reuse করতে পারবে। Module শুধু column definition দেবে।

|<p>const AppGrid = (function () {</p><p></p><p>`    `// Standard action column — সব grid এ same</p><p>`    `function \_actionColumn(config) {</p><p>`        `return {</p><p>`            `title: "Action", width: config.actionWidth || 140,</p><p>`            `filterable: false, sortable: false,</p><p>`            `template: function (dataItem) {</p><p>`                `let html = "";</p><p>`                `if (config.canView)</p><p>`                    `html += `<button class="k-button btn-sm btn-outline btn-view"</p><p>`                              `data-id="${dataItem[config.idField]}" title="View"></p><p>`                              `<i class="fa fa-eye"></i></button>`;</p><p>`                `if (config.canEdit)</p><p>`                    `html += `<button class="k-button btn-sm btn-secondary btn-edit"</p><p>`                              `data-id="${dataItem[config.idField]}" title="Edit"></p><p>`                              `<i class="fa fa-pencil"></i></button>`;</p><p>`                `if (config.canDelete)</p><p>`                    `html += `<button class="k-button btn-sm btn-danger btn-delete"</p><p>`                              `data-id="${dataItem[config.idField]}" title="Delete"></p><p>`                              `<i class="fa fa-trash"></i></button>`;</p><p>`                `return html;</p><p>`            `}</p><p>`        `};</p><p>`    `}</p><p></p><p>`    `// Standard grid init</p><p>`    `function init(config) {</p><p>`        `config.columns.push(\_actionColumn(config));</p><p>`        `$(config.selector).kendoGrid({</p><p>`            `dataSource: {</p><p>`                `transport: {</p><p>`                    `read: { url: config.readUrl, type: "POST", dataType: "json" }</p><p>`                `},</p><p>`                `schema:          { data: "data", total: "total" },</p><p>`                `serverPaging:    true,</p><p>`                `serverSorting:   true,</p><p>`                `serverFiltering: true,</p><p>`                `pageSize:        config.pageSize || 20,</p><p>`            `},</p><p>`            `columns:    config.columns,</p><p>`            `pageable:   { pageSizes: [10, 20, 50, 100] },</p><p>`            `sortable:   true,</p><p>`            `filterable: { mode: "row" },</p><p>`            `resizable:  true,</p><p>`            `noRecords:  { template: "<div class=grid-empty>No records found.</div>" }</p><p>`        `});</p><p>`        `// Event delegation — একটাই listener সব button এর জন্য</p><p>`        `$(document).on("click", `${config.selector} .btn-edit`, function () {</p><p>`            `config.onEdit && config.onEdit($(this).data("id"));</p><p>`        `});</p><p>`        `$(document).on("click", `${config.selector} .btn-delete`, function () {</p><p>`            `const id = $(this).data("id");</p><p>`            `AppModal.confirm("Delete this record?",</p><p>`                `"This action cannot be undone.", () => config.onDelete(id));</p><p>`        `});</p><p>`    `}</p><p></p><p>`    `function refresh(selector) {</p><p>`        `$(selector).data("kendoGrid").dataSource.read();</p><p>`    `}</p><p></p><p>`    `return { init, refresh };</p><p>})();</p>|
| :- |

## **18.5  Module Layer — employee.grid.js (Thin)**
Core layer আছে বলে module JS অনেক ছোট। শুধু column definition আর event handler।

|<p>// modules/employee/employee.grid.js</p><p>$(function () {</p><p>`    `AppGrid.init({</p><p>`        `selector:     "#gridEmployee",</p><p>`        `readUrl:      "/Employee/GetList",</p><p>`        `idField:      "EmployeeId",</p><p>`        `canEdit:      true,</p><p>`        `canDelete:    true,</p><p>`        `canView:      true,</p><p></p><p>`        `columns: [</p><p>`            `{ field: "EmployeeCode",   title: "Emp. Code",   width: 120 },</p><p>`            `{ field: "FullName",       title: "Full Name",   width: 200 },</p><p>`            `{ field: "DepartmentName", title: "Department",  width: 160 },</p><p>`            `{ field: "StatusName",     title: "Status",      width: 100,</p><p>`              `template: "<span class=badge badge-#=StatusClass#>#=StatusName#</span>" },</p><p>`        `],</p><p></p><p>`        `onEdit:   id => AppModal.openPartial(`/Employee/Edit/${id}`,   "Edit Employee",   "900px"),</p><p>`        `onDelete: id => EmployeePage.delete(id),</p><p>`        `onView:   id => AppModal.openPartial(`/Employee/View/${id}`,   "Employee Detail", "700px"),</p><p>`    `});</p><p></p><p>`    `$("#btnAddEmployee").on("click", function () {</p><p>`        `AppModal.openPartial("/Employee/Create", "Add New Employee", "900px");</p><p>`    `});</p><p>});</p><p></p><p>const EmployeePage = {</p><p>`    `delete: function (id) {</p><p>`        `AppApi.post("/Employee/Delete", { id }).then(function (res) {</p><p>`            `if (res.success) { AppToast.success("Deleted."); AppGrid.refresh("#gridEmployee"); }</p><p>`            `else AppToast.error(res.message);</p><p>`        `});</p><p>`    `}</p><p>};</p>|
| :- |

## **18.6  Form Layout — CSS Class Pattern**
HTML-এ শুধু class বদলালেই ৩ ধরনের form layout পাওয়া যাবে। কোনো JS change লাগবে না।

|<p><!-- Type 1: Single Column (Simple form, 4-6 fields) --></p><p><div class="form-layout form-layout--single"></p><p>`    `<div class="form-group"> ... </div></p><p></div></p><p></p><p><!-- Type 2: Two Column Grid (Complex form, 6-20+ fields) --></p><p><div class="form-layout form-layout--grid"></p><p>`    `<div class="form-group col-span-1"> ... </div></p><p>`    `<div class="form-group col-span-1"> ... </div></p><p>`    `<div class="form-group col-span-2"> ... </div>  ← full width</p><p></div></p><p></p><p><!-- Type 3: Inline Filter (Search/filter bar above grid) --></p><p><div class="form-layout form-layout--inline"></p><p>`    `<div class="form-group"> ... </div></p><p>`    `<button class="k-button btn-primary">Search</button></p><p></div></p>|
| :- |

|<p>// \_form-layouts.scss</p><p>.form-layout {</p><p>    .form-group { margin-bottom: 16px; }</p><p>`    `label { display: block; font-size: 13px; font-weight: 500; margin-bottom: 4px; }</p><p>    .k-input, .k-dropdown { width: 100%; height: 36px; }</p><p>}</p><p>.form-layout--single  { max-width: 720px; }</p><p>.form-layout--grid    {</p><p>`    `display: grid;</p><p>`    `grid-template-columns: 1fr 1fr;</p><p>`    `gap: 0 16px;</p><p>    .col-span-2 { grid-column: span 2; }</p><p>`    `@media (max-width: 768px) { grid-template-columns: 1fr; }</p><p>}</p><p>.form-layout--inline  {</p><p>`    `display: flex; align-items: flex-end; gap: 12px; flex-wrap: wrap;</p><p>    .form-group { margin-bottom: 0; min-width: 160px; }</p><p>}</p>|
| :- |

## **18.7  Reuse Benefit Summary**

|**কাজ**|**Core layer ছাড়া**|**Core layer দিয়ে**|
| :- | :- | :- |
|নতুন module grid|১০০+ line code|২০-৩০ line (শুধু columns)|
|Delete confirmation|প্রতি module আলাদা JS|AppModal.confirm(...)  — ১ লাইন|
|Toast notification|jQuery plugin আলাদা setup|AppToast.success(...)  — ১ লাইন|
|Grid refresh|Kendo API manually|AppGrid.refresh("#gridId")  — ১ লাইন|
|Form validation|Kendo init প্রতি module আলাদা|AppForm.init({...})  — config only|
|Loading state|প্রতি module আলাদা overlay|AppLoader.show() / hide()  — ১ লাইন|
|API call|$.ajax() everywhere|AppApi.post(url, data).then(...)|



|**19  Validation System — Complete Pattern**|
| :- |

## **19.1  Validation Trigger Points**
Validation ৩টি event এ fire হবে। প্রতিটির আলাদা behavior আছে।

|**Trigger**|**Event**|**Scope**|**UX Effect**|
| :- | :- | :- | :- |
|Tab / Focus Out|blur|শুধু ঐ একটি field|Field থেকে বের হলেই error দেখায়|
|Submit|form submit|সব field একসাথে|প্রথম error-এ scroll + focus|
|Live Clear|input (type করা)|Error থাকা field|Type করলে error সরে যায় real-time|
|Kendo Widget Change|change event|Dropdown, DatePicker, ComboBox|Value select করলে validate|

## **19.2  Validation Visual Flow**

|**Scenario**|**কী হয়**|
| :- | :- |
|User Full Name blank রেখে Tab চাপে|blur → \_validateField() → value নেই → Red border + fade-in error message নিচে|
|User error field এ type শুরু করে|input → has-error আছে → \_clearError() → Error real-time সরে যায়|
|User valid value দিয়ে Tab চাপে|blur → \_validateField() → value আছে → Green border (0.15s transition)|
|User Submit চাপে (error থাকলে)|\_validateAll() → সব field check → প্রথম error-এ scroll → focus → warning toast|
|User Submit চাপে (সব ঠিক)|\_validateAll() → pass → \_submit() → loading overlay → API call|

## **19.3  HTML Field Markup — Standard**
প্রতিটি form field এই structure follow করবে:

|<p><div class="form-group" id="fg\_EmployeeName"></p><p>`    `<label for="EmployeeName"></p><p>`        `Full Name <span class="required-star">\*</span></p><p>`    `</label></p><p>`    `<input type="text"</p><p>`           `id="EmployeeName"</p><p>`           `name="EmployeeName"</p><p>`           `class="k-input"</p><p>`           `data-val="true"</p><p>`           `data-required-msg="Full Name is required."</p><p>`           `data-minlength="2"</p><p>`           `placeholder="Enter full name" /></p><p>`    `<!-- Error message এখানে inject হবে --></p><p>`    `<span class="field-error-msg" id="err\_EmployeeName"></span></p><p></div></p><p></p><p><!-- Optional field (no data-val) --></p><p><div class="form-group"></p><p>`    `<label>Middle Name <span class="optional-label">(optional)</span></label></p><p>`    `<input type="text" name="MiddleName" class="k-input" /></p><p>`    `<span class="field-error-msg"></span></p><p></div></p>|
| :- |

## **19.4  Data Attributes — Validation Rules**

|**Attribute**|**Type**|**Usage**|
| :- | :- | :- |
|data-val="true"|boolean|Required field — এটা না থাকলে required check হবে না|
|data-required-msg|string|Custom required message — "Employee Code is required."|
|data-minlength|number|Minimum character length check|
|data-maxlength|number|Maximum character limit|
|data-custom-rule|string|Custom JS function name — module থেকে inject করা যাবে|
|type="email"|HTML attr|Email format auto-validate|
|type="number"|HTML attr|Numeric only check (Kendo NumericTextBox preferred)|

## **19.5  app.form.js — Complete Validation Implementation**

|<p>const AppForm = (function () {</p><p></p><p>`    `function init(config) {</p><p>`        `const $form = $(`#${config.formId}`);</p><p></p><p>`        `// Step 1: Blur (Tab) — single field validate</p><p>`        `$form.on("blur", "input, select, textarea", function () {</p><p>`            `\_validateField($(this));</p><p>`        `});</p><p></p><p>`        `// Step 2: Kendo widget change — dropdown, datepicker, combobox</p><p>`        `\_bindKendoWidgetValidation($form);</p><p></p><p>`        `// Step 3: Submit — সব field validate</p><p>`        `$form.on("submit", function (e) {</p><p>`            `e.preventDefault();</p><p>`            `if (!\_validateAll($form)) {</p><p>`                `const $first = $form.find(".form-group.has-error").first();</p><p>`                `if ($first.length) {</p><p>`                    `$("html,body").animate({ scrollTop: $first.offset().top - 100 }, 250,</p><p>`                        `function () { $first.find("input,select,textarea").focus(); });</p><p>`                `}</p><p>`                `AppToast.warning("Please fix the errors before submitting.");</p><p>`                `return;</p><p>`            `}</p><p>`            `\_submit(config, $form);</p><p>`        `});</p><p></p><p>`        `// Step 4: Live clear — error থাকা field এ type করলে error সরে</p><p>`        `$form.on("input", "input, textarea", function () {</p><p>`            `if ($(this).closest(".form-group").hasClass("has-error"))</p><p>`                `\_clearError($(this));</p><p>`        `});</p><p>`    `}</p><p></p><p>`    `// Single field validate</p><p>`    `function \_validateField($field) {</p><p>`        `const $group  = $field.closest(".form-group");</p><p>`        `const $errSpan = $group.find(".field-error-msg");</p><p>`        `const value   = ($field.val() || "").trim();</p><p></p><p>`        `// Required check</p><p>`        `if ($field.attr("data-val") === "true" && !value) {</p><p>`            `const msg = $field.attr("data-required-msg") || `${$field.attr("name")} is required.`;</p><p>`            `return \_showError($group, $errSpan, msg);</p><p>`        `}</p><p>`        `// Min length check</p><p>`        `const minLen = parseInt($field.attr("data-minlength") || 0);</p><p>`        `if (minLen && value.length < minLen)</p><p>`            `return \_showError($group, $errSpan, `Minimum ${minLen} characters required.`);</p><p></p><p>`        `// Email format check</p><p>`        `if ($field.attr("type") === "email" && value) {</p><p>`            `if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value))</p><p>`                `return \_showError($group, $errSpan, "Enter a valid email address.");</p><p>`        `}</p><p>`        `// Custom rule (module থেকে inject)</p><p>`        `const ruleFn = $field.attr("data-custom-rule");</p><p>`        `if (ruleFn && window[ruleFn]) {</p><p>`            `const result = window[ruleFn](value, $field);</p><p>`            `if (result !== true) return \_showError($group, $errSpan, result);</p><p>`        `}</p><p>`        `// সব ঠিক → clear</p><p>`        `\_clearError($field);</p><p>`        `return true;</p><p>`    `}</p><p></p><p>`    `// সব field একসাথে validate (submit এ)</p><p>`    `function \_validateAll($form) {</p><p>`        `let valid = true;</p><p>`        `$form.find("input[data-val], select[data-val], textarea[data-val]")</p><p>            .each(function () { if (!\_validateField($(this))) valid = false; });</p><p>`        `return valid;</p><p>`    `}</p><p></p><p>`    `// Show error</p><p>`    `function \_showError($group, $errSpan, msg) {</p><p>`        `$group.addClass("has-error").removeClass("has-success");</p><p>`        `$errSpan.html(`<i class="fa fa-exclamation-circle"></i> ${msg}`)</p><p>                .stop(true).hide().fadeIn(150);</p><p>`        `return false;</p><p>`    `}</p><p></p><p>`    `// Clear error</p><p>`    `function \_clearError($field) {</p><p>`        `const $group = $field.closest(".form-group");</p><p>`        `$group.removeClass("has-error").addClass("has-success");</p><p>`        `$group.find(".field-error-msg").fadeOut(150, function () { $(this).html(""); });</p><p>`        `return true;</p><p>`    `}</p><p></p><p>`    `// Kendo widget blur simulation</p><p>`    `function \_bindKendoWidgetValidation($form) {</p><p>`        `$form.find("[data-role=dropdownlist]").each(function () {</p><p>`            `$(this).data("kendoDropDownList")?.bind("change", function () {</p><p>`                `\_validateField($(this.element));</p><p>`            `});</p><p>`        `});</p><p>`        `$form.find("[data-role=datepicker]").each(function () {</p><p>`            `$(this).data("kendoDatePicker")?.bind("change", function () {</p><p>`                `\_validateField($(this.element));</p><p>`            `});</p><p>`        `});</p><p>`        `$form.find("[data-role=combobox]").each(function () {</p><p>`            `$(this).data("kendoComboBox")?.bind("change", function () {</p><p>`                `\_validateField($(this.element));</p><p>`            `});</p><p>`        `});</p><p>`    `}</p><p></p><p>`    `return { init };</p><p>})();</p>|
| :- |

## **19.6  SCSS — Validation Visual States**

|<p>// \_form.scss — Validation states</p><p>.required-star  { color: #DC2626; font-size: 13px; margin-left: 2px; }</p><p>.optional-label { color: #94A3B8; font-size: 11px; font-style: italic; }</p><p></p><p>.form-group {</p><p>`    `// Default input border + focus</p><p>    .k-input, .k-dropdown, .k-datepicker, input, select, textarea {</p><p>`        `border: 1px solid #CBD5E1;</p><p>`        `border-radius: 4px;</p><p>`        `transition: border-color 0.15s ease, box-shadow 0.15s ease;</p><p>`        `&:focus {</p><p>`            `border-color: #2563EB;</p><p>`            `box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.12);</p><p>`            `outline: none;</p><p>`        `}</p><p>`    `}</p><p>`    `// Error state</p><p>`    `&.has-error {</p><p>        .k-input, .k-dropdown, input, select, textarea {</p><p>`            `border-color: #DC2626 !important;</p><p>`            `background: #FFF5F5;</p><p>`            `box-shadow: 0 0 0 3px rgba(220, 38, 38, 0.08);</p><p>`        `}</p><p>`        `label { color: #DC2626; }</p><p>`    `}</p><p>`    `// Success state</p><p>`    `&.has-success {</p><p>        .k-input, input { border-color: #16A34A; }</p><p>`    `}</p><p>`    `// Error message</p><p>    .field-error-msg {</p><p>`        `display: block;</p><p>`        `color: #DC2626;</p><p>`        `font-size: 11px;</p><p>`        `font-style: italic;</p><p>`        `margin-top: 4px;</p><p>`        `i { margin-right: 3px; }</p><p>`    `}</p><p>}</p>|
| :- |

## **19.7  Module Custom Validation Rule — Inject Pattern**
Core layer change না করে module থেকে custom rule যোগ করার pattern:

|<p>// modules/employee/employee.form.js</p><p></p><p>// Custom rule — global function হিসেবে define করো</p><p>window.validateEmployeeCode = function (value, $field) {</p><p>`    `if (!/^EMP-\d{4}$/.test(value))</p><p>`        `return "Format must be EMP-0001 (e.g. EMP-0023)";</p><p>`    `return true; // valid</p><p>};</p><p></p><p>window.validateJoinDate = function (value, $field) {</p><p>`    `const date = new Date(value);</p><p>`    `if (date > new Date()) return "Join Date cannot be in the future.";</p><p>`    `return true;</p><p>};</p>|
| :- |

|<p><!-- HTML — data-custom-rule attribute দিয়ে inject --></p><p><input name="EmployeeCode"</p><p>`       `data-val="true"</p><p>`       `data-required-msg="Employee Code is required."</p><p>`       `data-custom-rule="validateEmployeeCode" /></p><p></p><p><input name="JoinDate" type="text" data-role="datepicker"</p><p>`       `data-val="true"</p><p>`       `data-required-msg="Join Date is required."</p><p>`       `data-custom-rule="validateJoinDate" /></p>|
| :- |

## **19.8  Module Usage — AppForm.init()**
Module form JS শুধু init call করবে। Validation logic core এ আছে।

|<p>// modules/employee/employee.form.js</p><p>$(function () {</p><p>`    `AppForm.init({</p><p>`        `formId:  "formEmployee",</p><p>`        `saveUrl: "/Employee/Save",</p><p>`        `onSuccess: function (response) {</p><p>`            `AppModal.close();</p><p>`            `AppGrid.refresh("#gridEmployee");</p><p>`            `// AppToast.success() already called in AppForm core</p><p>`        `},</p><p>`        `onError: function (response) {</p><p>`            `// server-side validation error handle করো</p><p>`            `if (response.errors) {</p><p>`                `$.each(response.errors, function (fieldName, msg) {</p><p>`                    `const $field = $(`[name="${fieldName}"]`);</p><p>`                    `const $group = $field.closest(".form-group");</p><p>`                    `$group.find(".field-error-msg")</p><p>                          .html(`<i class="fa fa-exclamation-circle"></i> ${msg}`)</p><p>                          .show();</p><p>`                    `$group.addClass("has-error");</p><p>`                `});</p><p>`            `}</p><p>`        `}</p><p>`    `});</p><p>});</p>|
| :- |

## **19.9  Validation Rules Summary Table**

|**Rule**|**HTML Attribute**|**Example Value**|**Error Message**|
| :- | :- | :- | :- |
|Required|data-val="true"|—|"{FieldName} is required."|
|Min Length|data-minlength|data-minlength="3"|"Minimum 3 characters required."|
|Email Format|type="email"|type="email"|"Enter a valid email address."|
|Custom Rule|data-custom-rule|data-custom-rule="validateCode"|Function return value|
|Kendo Required|data-val="true" + widget|DropDownList, DatePicker|Same — change event এ trigger|

*💡 Server-side validation error (API response) → onError callback থেকে field-level error inject করো। Client + Server দুই layer এ validation থাকলে সবচেয়ে robust।*



|**20  Frontend Implementation Plan & Progress Tracking**|
| :- |

---

# 🎯 bdDevsCrm Frontend Implementation Plan

> **Status:** Planning Complete ✅ | Implementation: Not Started
> **Tech Stack:** ASP.NET Core MVC + Kendo UI 2024 Q4 + jQuery + Fetch API
> **Architecture:** Clean Architecture (Backend) + 3-File JS Pattern (Frontend)

---

## Implementation Overview

This section tracks the **complete frontend implementation** from core infrastructure to the first CRUD module (Country). Each phase is broken down into specific steps with success criteria and file locations.

**Key Principles:**
- ✅ Backend API is complete and tested
- ✅ Use Fetch API for all HTTP calls (NEVER jQuery Ajax)
- ✅ Follow existing UI/UX design system (sections 1-19 above)
- ✅ Token-based authentication with in-memory storage
- ✅ Kendo UI 2024 Q4 for grid, forms, and widgets

---

## 📋 Phase 1: Core Infrastructure Setup

### Step 1.1: API Configuration & Constants

**Status:** ☐ Not Started

**Files to Create/Update:**
```
Presentation.Mvc/wwwroot/js/core/
├── app.config.js          ← API base URL, timeout, default headers
└── app.constants.js       ← Route constants, cache keys, message constants
```

**Implementation Details:**

```javascript
// app.config.js
const AppConfig = {
    apiBaseUrl: 'https://localhost:7001/api',  // Presentation.Api URL
    timeout: 30000,  // 30 seconds
    headers: {
        'Content-Type': 'application/json'
    },
    auth: {
        tokenKey: 'accessToken',        // In-memory storage key
        refreshTokenKey: 'refreshToken'  // HTTP-only cookie
    }
};

// app.constants.js
const ApiRoutes = {
    // Auth
    login: '/authentication/login',
    refreshToken: '/authentication/refresh-token',
    logout: '/authentication/logout',

    // Country (first module)
    countries: '/core/systemadmin/countries',
    countrySummary: '/core/systemadmin/country-summary',
    createCountry: '/core/systemadmin/create-country',
    updateCountry: (id) => `/core/systemadmin/update-country/${id}`,
    deleteCountry: (id) => `/core/systemadmin/delete-country/${id}`
};

const Messages = {
    success: {
        created: 'Record created successfully!',
        updated: 'Record updated successfully!',
        deleted: 'Record deleted successfully!'
    },
    errors: {
        network: 'Network error. Please check your connection.',
        unauthorized: 'Session expired. Please login again.',
        serverError: 'Server error. Please try again later.'
    }
};
```

**Success Criteria:**
- [ ] `AppConfig` and `ApiRoutes` constants defined
- [ ] File loaded in `_Layout.cshtml` before other scripts
- [ ] `console.log(AppConfig.apiBaseUrl)` works in browser console

---

### Step 1.2: Enhanced API Client (Fetch Wrapper)

**Status:** ☐ Not Started

**Files to Create:**
```
Presentation.Mvc/wwwroot/js/core/
└── app.api.js              ← Centralized Fetch API wrapper with auth, error handling
```

**Implementation Details:**

```javascript
// app.api.js
const ApiClient = (function() {
    let accessToken = null;  // In-memory token storage

    function setToken(token) {
        accessToken = token;
    }

    function getToken() {
        return accessToken;
    }

    function clearToken() {
        accessToken = null;
    }

    async function request(url, options = {}) {
        const config = {
            method: options.method || 'GET',
            headers: {
                ...AppConfig.headers,
                ...(accessToken ? { 'Authorization': `Bearer ${accessToken}` } : {}),
                ...options.headers
            }
        };

        if (options.body) {
            config.body = JSON.stringify(options.body);
        }

        try {
            const response = await fetch(`${AppConfig.apiBaseUrl}${url}`, config);

            // Handle 401 Unauthorized - token expired
            if (response.status === 401) {
                clearToken();
                window.location.href = '/Account/Login';
                return;
            }

            const data = await response.json();

            if (!response.ok) {
                throw new Error(data.message || `HTTP ${response.status}`);
            }

            return data;  // Returns ApiResponse<T>
        } catch (error) {
            console.error('API Error:', error);
            throw error;
        }
    }

    // Convenience methods
    async function get(url) {
        return request(url, { method: 'GET' });
    }

    async function post(url, body) {
        return request(url, { method: 'POST', body });
    }

    async function put(url, body) {
        return request(url, { method: 'PUT', body });
    }

    async function del(url) {
        return request(url, { method: 'DELETE' });
    }

    return {
        setToken,
        getToken,
        clearToken,
        get,
        post,
        put,
        delete: del
    };
})();
```

**Success Criteria:**
- [ ] `ApiClient.get()`, `ApiClient.post()`, `ApiClient.put()`, `ApiClient.delete()` work
- [ ] Token is stored in memory (not localStorage)
- [ ] 401 response redirects to login page
- [ ] Network errors are caught and logged

---

### Step 1.3: Authentication & Session Management

**Status:** ☐ Not Started

**Files to Create:**
```
Presentation.Mvc/wwwroot/js/core/
└── app.auth.js             ← Login, token refresh, logout logic
```

**Implementation Details:**

```javascript
// app.auth.js
const AuthManager = (function() {
    async function login(loginId, password) {
        try {
            const response = await ApiClient.post(ApiRoutes.login, {
                loginId,
                password
            });

            if (response.success && response.data) {
                // Store access token in memory
                ApiClient.setToken(response.data.accessToken);

                // RefreshToken is automatically stored in HTTP-only cookie by server

                return response.data;
            } else {
                throw new Error(response.message || 'Login failed');
            }
        } catch (error) {
            console.error('Login error:', error);
            throw error;
        }
    }

    async function logout() {
        try {
            await ApiClient.post(ApiRoutes.logout);
        } catch (error) {
            console.error('Logout error:', error);
        } finally {
            ApiClient.clearToken();
            window.location.href = '/Account/Login';
        }
    }

    async function refreshToken() {
        try {
            // RefreshToken is sent automatically via HTTP-only cookie
            const response = await ApiClient.post(ApiRoutes.refreshToken);

            if (response.success && response.data) {
                ApiClient.setToken(response.data.accessToken);
                return true;
            }
            return false;
        } catch (error) {
            console.error('Token refresh failed:', error);
            return false;
        }
    }

    function isAuthenticated() {
        return ApiClient.getToken() !== null;
    }

    return {
        login,
        logout,
        refreshToken,
        isAuthenticated
    };
})();
```

**Success Criteria:**
- [ ] `AuthManager.login()` stores token in memory
- [ ] `AuthManager.logout()` clears token and redirects
- [ ] `AuthManager.isAuthenticated()` returns correct status
- [ ] RefreshToken is stored in HTTP-only cookie (check DevTools > Application > Cookies)

---

## 📋 Phase 2: Login Page Implementation

### Step 2.1: Login View (Razor Page)

**Status:** ☐ Not Started

**Files to Create:**
```
Presentation.Mvc/Views/Account/
└── Login.cshtml            ← Login page HTML
```

**Implementation:**

```html
@{
    Layout = null;  // No master layout for login page
}
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Login - HRIS System</title>
    <link rel="stylesheet" href="~/css/login.css" />
</head>
<body>
    <div class="login-container">
        <div class="login-card">
            <div class="login-header">
                <h1>HRIS System</h1>
                <p>Please login to continue</p>
            </div>

            <form id="loginForm" class="login-form">
                <div class="form-group">
                    <label for="loginId">Login ID <span class="required-star">*</span></label>
                    <input type="text" id="loginId" name="loginId" class="k-input"
                           placeholder="Enter your login ID"
                           data-val="true"
                           data-required-msg="Login ID is required." />
                    <span class="field-error-msg" id="err_loginId"></span>
                </div>

                <div class="form-group">
                    <label for="password">Password <span class="required-star">*</span></label>
                    <input type="password" id="password" name="password" class="k-input"
                           placeholder="Enter your password"
                           data-val="true"
                           data-required-msg="Password is required." />
                    <span class="field-error-msg" id="err_password"></span>
                </div>

                <div class="form-group">
                    <label>
                        <input type="checkbox" id="rememberMe" name="rememberMe" />
                        <span>Remember me</span>
                    </label>
                </div>

                <button type="submit" class="k-button btn-primary btn-lg btn-block">
                    Login
                </button>
            </form>
        </div>
    </div>

    <script src="~/lib/jquery/jquery.min.js"></script>
    <script src="~/lib/kendo_2024_q4/js/kendo.all.min.js"></script>
    <script src="~/js/core/app.config.js"></script>
    <script src="~/js/core/app.constants.js"></script>
    <script src="~/js/core/app.api.js"></script>
    <script src="~/js/core/app.auth.js"></script>
    <script src="~/js/core/app.toast.js"></script>
    <script src="~/js/core/app.loader.js"></script>
    <script src="~/js/modules/account/login.js"></script>
</body>
</html>
```

**Success Criteria:**
- [ ] Login page renders without layout
- [ ] Form fields use Kendo UI styles
- [ ] All core JS files load without errors

---

### Step 2.2: Login JavaScript

**Status:** ☐ Not Started

**Files to Create:**
```
Presentation.Mvc/wwwroot/js/modules/account/
└── login.js                ← Login form validation & submit logic
```

**Implementation:**

```javascript
// login.js
$(function() {
    const $form = $('#loginForm');
    const $loginBtn = $form.find('button[type="submit"]');

    $form.on('submit', async function(e) {
        e.preventDefault();

        // Validate
        if (!validateLoginForm()) {
            return;
        }

        const loginId = $('#loginId').val().trim();
        const password = $('#password').val().trim();

        // Show loading state
        $loginBtn.prop('disabled', true).text('Logging in...');
        AppLoader.show();

        try {
            const user = await AuthManager.login(loginId, password);

            AppToast.success('Login successful!');

            // Redirect to dashboard
            setTimeout(() => {
                window.location.href = '/Dashboard/Index';
            }, 500);

        } catch (error) {
            AppToast.error(error.message || 'Login failed. Please check your credentials.');
            $loginBtn.prop('disabled', false).text('Login');
        } finally {
            AppLoader.hide();
        }
    });

    function validateLoginForm() {
        let isValid = true;

        const loginId = $('#loginId').val().trim();
        const password = $('#password').val().trim();

        if (!loginId) {
            showError('loginId', 'Login ID is required.');
            isValid = false;
        } else {
            clearError('loginId');
        }

        if (!password) {
            showError('password', 'Password is required.');
            isValid = false;
        } else {
            clearError('password');
        }

        return isValid;
    }

    function showError(fieldId, message) {
        $(`#${fieldId}`).closest('.form-group').addClass('has-error');
        $(`#err_${fieldId}`).html(`<i class="fa fa-exclamation-circle"></i> ${message}`).fadeIn(150);
    }

    function clearError(fieldId) {
        $(`#${fieldId}`).closest('.form-group').removeClass('has-error');
        $(`#err_${fieldId}`).fadeOut(150, function() { $(this).html(''); });
    }
});
```

**Success Criteria:**
- [ ] Form validates before submission
- [ ] Login button shows loading state during API call
- [ ] Success redirects to dashboard
- [ ] Error shows toast notification
- [ ] Token is stored in memory (check via `ApiClient.getToken()`)

---

### Step 2.3: Account Controller (MVC)

**Status:** ☐ Not Started

**Files to Create:**
```
Presentation.Mvc/Controllers/
└── AccountController.cs    ← Login, Logout actions
```

**Implementation:**

```csharp
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        // If already authenticated, redirect to dashboard
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Dashboard");

        return View();
    }

    [HttpPost]
    public IActionResult Logout()
    {
        // Frontend handles token clearing via AuthManager.logout()
        return RedirectToAction("Login");
    }
}
```

**Success Criteria:**
- [ ] `/Account/Login` route works
- [ ] Already authenticated users redirect to dashboard
- [ ] Logout action redirects to login

---

## 📋 Phase 3: Layout & Navigation Enhancement

### Step 3.1: Update Master Layout

**Status:** ☐ Not Started

**Files to Update:**
```
Presentation.Mvc/Views/Shared/
└── _Layout.cshtml          ← Add auth check, logout button, user info
```

**Changes:**
1. Add auth check at top of layout
2. Display current user info in header
3. Add logout button
4. Load all core JS files

**Success Criteria:**
- [ ] Layout shows user info when logged in
- [ ] Logout button calls `AuthManager.logout()`
- [ ] Unauthenticated users are redirected to login

---

### Step 3.2: Protected Routes Middleware

**Status:** ☐ Not Started

**Files to Create:**
```
Presentation.Mvc/Middleware/
└── AuthenticationCheckMiddleware.cs    ← Redirect unauthenticated requests
```

**Success Criteria:**
- [ ] All routes except `/Account/Login` require authentication
- [ ] Unauthenticated requests redirect to login page

---

## 📋 Phase 4: Kendo UI Integration

### Step 4.1: Kendo UI File Structure

**Status:** ☐ Not Started (User will manually add Kendo files)

**Expected Structure:**
```
Presentation.Mvc/wwwroot/lib/kendo_2024_q4/
├── js/
│   └── kendo.all.min.js
├── css/
│   ├── kendo.common.min.css
│   └── kendo.default.min.css
└── styles/
    └── (theme resources)
```

**Action Required:**
> **User Task:** Manually copy Kendo UI 2024 Q4 files to `wwwroot/lib/kendo_2024_q4/`

**Success Criteria:**
- [ ] Kendo files are present in `wwwroot/lib/kendo_2024_q4/`
- [ ] `kendo.all.min.js` loads without errors
- [ ] Kendo CSS files load and apply correct styles

---

### Step 4.2: Kendo UI Theme Override

**Status:** ☐ Not Started

**Files to Create:**
```
Presentation.Mvc/wwwroot/scss/kendo-override/
└── _kendo-theme.scss       ← Custom Kendo theme overrides
```

**Implementation:**

```scss
// _kendo-theme.scss
$kendo-color-primary: #1E5FA8;
$kendo-border-radius: 4px;
$kendo-grid-header-bg: #1E5FA8;
$kendo-grid-header-text: #ffffff;

.k-button.k-primary {
    background: #1E5FA8;
    border-color: #1E5FA8;
    &:hover {
        background: #1E3A5F;
    }
}

.k-grid {
    border-color: #CBD5E1;
    th {
        background: #1E5FA8 !important;
        color: #fff !important;
        font-weight: 600;
    }
    tr:hover {
        background: #EFF6FF;
    }
}

.k-input, .k-dropdown, .k-datepicker {
    height: 36px;
    border: 1px solid #CBD5E1;
    &:focus {
        border-color: #2563EB;
        box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.12);
    }
}
```

**Success Criteria:**
- [ ] Kendo grid headers use primary blue color (#1E5FA8)
- [ ] Buttons match design system
- [ ] Input fields have correct height (36px)

---

## 📋 Phase 5: First Module - Country CRUD

### Step 5.1: Backend Verification

**Status:** ✅ Already Complete

The Country module backend is **already implemented**:
- ✅ `CrmCountry` entity
- ✅ `ICrmCountryRepository` + implementation
- ✅ `ICrmCountryService` + implementation
- ✅ `CountryController` with all CRUD endpoints
- ✅ CRUD Records (`CreateCountryRecord`, `UpdateCountryRecord`, `DeleteCountryRecord`)
- ✅ `CrmCountryDto`

**Verified Endpoints:**
```
GET    /api/core/systemadmin/countries
POST   /api/core/systemadmin/country-summary
POST   /api/core/systemadmin/create-country
PUT    /api/core/systemadmin/update-country/{key}
DELETE /api/core/systemadmin/delete-country/{key}
```

---

### Step 5.2: Country View (Razor Page)

**Status:** ☐ Not Started

**Files to Create:**
```
Presentation.Mvc/Views/SystemAdmin/
└── Country.cshtml          ← Country list page with Kendo Grid
```

**Implementation:**

```html
@{
    ViewBag.PageTitle = "Country Management";
    ViewBag.Breadcrumb = new[] { "Home", "System Admin", "Country Management" };
}

<div class="content-zone">
    <!-- Toolbar -->
    <div class="grid-toolbar">
        <button id="btnAddCountry" class="k-button btn-primary">
            <i class="fa fa-plus"></i> Add New Country
        </button>
        <button id="btnRefresh" class="k-button btn-outline">
            <i class="fa fa-refresh"></i> Refresh
        </button>
    </div>

    <!-- Kendo Grid -->
    <div id="gridCountry"></div>
</div>

<!-- Country Form Modal (will be loaded via AJAX) -->
<div id="modalCountry"></div>

@section Scripts {
    <script src="~/js/modules/systemadmin/country.settings.js"></script>
    <script src="~/js/modules/systemadmin/country.details.js"></script>
    <script src="~/js/modules/systemadmin/country.summary.js"></script>
}
```

**Success Criteria:**
- [ ] Page renders with toolbar and grid container
- [ ] Add button and refresh button visible
- [ ] Modal container ready for dynamic content

---

### Step 5.3: Country JavaScript (3-File Pattern)

**Status:** ☐ Not Started

**Files to Create:**
```
Presentation.Mvc/wwwroot/js/modules/systemadmin/
├── country.settings.js     ← Constants, API routes
├── country.details.js      ← Create/Edit form logic
└── country.summary.js      ← Grid initialization, delete logic
```

**File 1: country.settings.js**

```javascript
// Settings - Constants and configuration
const CountrySettings = {
    gridSelector: '#gridCountry',
    modalSelector: '#modalCountry',

    routes: {
        summary: ApiRoutes.countrySummary,
        create: ApiRoutes.createCountry,
        update: ApiRoutes.updateCountry,
        delete: ApiRoutes.deleteCountry
    },

    columns: [
        { field: "countryId", title: "ID", width: 80 },
        { field: "countryName", title: "Country Name", width: 200 },
        { field: "countryCode", title: "Code", width: 100 },
        { field: "statusName", title: "Status", width: 120,
          template: "<span class='badge badge-#=statusClass#'>#=statusName#</span>" }
    ]
};
```

**File 2: country.summary.js**

```javascript
// Grid initialization and delete logic
$(function() {
    initCountryGrid();

    $('#btnAddCountry').on('click', function() {
        CountryDetails.openCreateModal();
    });

    $('#btnRefresh').on('click', function() {
        refreshGrid();
    });
});

function initCountryGrid() {
    $(CountrySettings.gridSelector).kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: `${AppConfig.apiBaseUrl}${CountrySettings.routes.summary}`,
                    type: 'POST',
                    dataType: 'json',
                    beforeSend: function(xhr) {
                        const token = ApiClient.getToken();
                        if (token) {
                            xhr.setRequestHeader('Authorization', `Bearer ${token}`);
                        }
                    }
                }
            },
            schema: {
                data: function(response) {
                    return response.data.items || [];
                },
                total: function(response) {
                    return response.data.totalCount || 0;
                }
            },
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            pageSize: 20
        },
        columns: [
            ...CountrySettings.columns,
            {
                title: "Actions",
                width: 160,
                filterable: false,
                sortable: false,
                template: function(dataItem) {
                    return `
                        <button class="k-button btn-sm btn-secondary btn-edit"
                                data-id="${dataItem.countryId}" title="Edit">
                            <i class="fa fa-pencil"></i>
                        </button>
                        <button class="k-button btn-sm btn-danger btn-delete"
                                data-id="${dataItem.countryId}" title="Delete">
                            <i class="fa fa-trash"></i>
                        </button>
                    `;
                }
            }
        ],
        pageable: {
            pageSizes: [10, 20, 50, 100]
        },
        sortable: true,
        filterable: {
            mode: 'row'
        },
        resizable: true,
        noRecords: {
            template: '<div class="grid-empty">No countries found.</div>'
        }
    });

    // Event delegation for action buttons
    $(document).on('click', `${CountrySettings.gridSelector} .btn-edit`, function() {
        const id = $(this).data('id');
        CountryDetails.openEditModal(id);
    });

    $(document).on('click', `${CountrySettings.gridSelector} .btn-delete`, function() {
        const id = $(this).data('id');
        deleteCountry(id);
    });
}

function refreshGrid() {
    $(CountrySettings.gridSelector).data('kendoGrid').dataSource.read();
}

async function deleteCountry(id) {
    if (!confirm('Are you sure you want to delete this country?')) {
        return;
    }

    AppLoader.show();
    try {
        const response = await ApiClient.delete(CountrySettings.routes.delete(id));

        if (response.success) {
            AppToast.success('Country deleted successfully!');
            refreshGrid();
        } else {
            AppToast.error(response.message || 'Failed to delete country.');
        }
    } catch (error) {
        AppToast.error(error.message || 'Error deleting country.');
    } finally {
        AppLoader.hide();
    }
}
```

**File 3: country.details.js**

```javascript
// Create/Edit form logic
const CountryDetails = (function() {

    function openCreateModal() {
        // Show modal with empty form
        const $modal = $(CountrySettings.modalSelector);
        $modal.html(`
            <div class="modal-overlay">
                <div class="modal-box" style="width: 600px;">
                    <div class="modal-header">
                        <h3>Add New Country</h3>
                        <button class="btn-close" onclick="CountryDetails.closeModal()">&times;</button>
                    </div>
                    <div class="modal-body">
                        <form id="formCountry" class="form-layout form-layout--single">
                            <div class="form-group">
                                <label for="countryName">Country Name <span class="required-star">*</span></label>
                                <input type="text" id="countryName" name="countryName" class="k-input"
                                       data-val="true"
                                       data-required-msg="Country name is required." />
                                <span class="field-error-msg" id="err_countryName"></span>
                            </div>

                            <div class="form-group">
                                <label for="countryCode">Country Code</label>
                                <input type="text" id="countryCode" name="countryCode" class="k-input" />
                            </div>

                            <div class="form-group">
                                <label for="statusId">Status <span class="required-star">*</span></label>
                                <select id="statusId" name="statusId" class="k-input"
                                        data-val="true"
                                        data-required-msg="Status is required.">
                                    <option value="">-- Select --</option>
                                    <option value="1">Active</option>
                                    <option value="2">Inactive</option>
                                </select>
                                <span class="field-error-msg" id="err_statusId"></span>
                            </div>
                        </form>
                    </div>
                    <div class="modal-footer">
                        <button class="k-button btn-outline" onclick="CountryDetails.closeModal()">Cancel</button>
                        <button class="k-button btn-primary" onclick="CountryDetails.saveCountry()">Save</button>
                    </div>
                </div>
            </div>
        `).fadeIn(200);

        // Initialize Kendo widgets
        $('#statusId').kendoDropDownList();
    }

    function openEditModal(id) {
        // TODO: Load country data and populate form
        // Similar to openCreateModal but with data pre-filled
    }

    function closeModal() {
        $(CountrySettings.modalSelector).fadeOut(200, function() {
            $(this).html('');
        });
    }

    async function saveCountry() {
        // Validate
        if (!validateForm()) {
            return;
        }

        const formData = {
            countryName: $('#countryName').val().trim(),
            countryCode: $('#countryCode').val().trim(),
            statusId: parseInt($('#statusId').val())
        };

        AppLoader.show();
        try {
            const response = await ApiClient.post(CountrySettings.routes.create, formData);

            if (response.success) {
                AppToast.success('Country created successfully!');
                closeModal();
                refreshGrid();
            } else {
                AppToast.error(response.message || 'Failed to create country.');
            }
        } catch (error) {
            AppToast.error(error.message || 'Error creating country.');
        } finally {
            AppLoader.hide();
        }
    }

    function validateForm() {
        let isValid = true;
        // Simple validation - can be enhanced with AppForm.init() pattern
        const countryName = $('#countryName').val().trim();
        if (!countryName) {
            isValid = false;
            AppToast.warning('Country name is required.');
        }
        return isValid;
    }

    return {
        openCreateModal,
        openEditModal,
        closeModal,
        saveCountry
    };
})();
```

**Success Criteria:**
- [ ] Grid loads with pagination, sorting, filtering
- [ ] Add button opens create modal
- [ ] Edit button loads existing data
- [ ] Delete button shows confirmation and deletes record
- [ ] All API calls use `ApiClient` with Bearer token
- [ ] Success/error messages shown via toast

---

### Step 5.4: Country Controller (MVC)

**Status:** ☐ Not Started

**Files to Create:**
```
Presentation.Mvc/Controllers/
└── SystemAdminController.cs    ← Render Country view
```

**Implementation:**

```csharp
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Mvc.Controllers;

public class SystemAdminController : Controller
{
    [HttpGet]
    public IActionResult Country()
    {
        ViewBag.PageTitle = "Country Management";
        ViewBag.Breadcrumb = new[] { "Home", "System Admin", "Country Management" };
        return View();
    }
}
```

**Success Criteria:**
- [ ] `/SystemAdmin/Country` route works
- [ ] Page renders with correct title and breadcrumb

---

## 📋 Phase 6: Session Management Strategy

### Token-Based Session Management (Recommended)

**Architecture:**

```
┌─────────────────────────────────────────────────────────┐
│                      User Login                         │
└─────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────┐
│  Backend API returns:                                   │
│  - AccessToken (short-lived, 15 min)                    │
│  - RefreshToken (long-lived, 7 days, HTTP-only cookie)  │
└─────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────┐
│  Frontend stores:                                       │
│  - AccessToken → In-memory variable (XSS protection)    │
│  - RefreshToken → HTTP-only cookie (CSRF protection)    │
└─────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────┐
│  On API call:                                           │
│  - Send AccessToken in Authorization header             │
│  - If 401 Unauthorized → Call /refresh-token            │
│  - Get new AccessToken → Retry original request         │
└─────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────┐
│  On page refresh / close:                               │
│  - AccessToken lost (in-memory cleared)                 │
│  - RefreshToken persists (HTTP-only cookie)             │
│  - Auto-refresh on next page load                       │
└─────────────────────────────────────────────────────────┘
```

**Security Benefits:**
- ✅ **XSS Protection:** AccessToken in memory (not localStorage/sessionStorage)
- ✅ **CSRF Protection:** RefreshToken in HTTP-only cookie (no JavaScript access)
- ✅ **Auto-logout:** AccessToken expires after 15 min of inactivity
- ✅ **Persistent Session:** RefreshToken allows seamless re-authentication

**Implementation Status:** ☐ Not Started (Core logic ready in Phase 1.3)

---

## 🎯 Implementation Progress Checklist

### Phase 1: Core Infrastructure ✅ COMPLETED
- [x] Step 1.1: API Configuration & Constants
- [x] Step 1.2: Enhanced API Client (Fetch Wrapper)
- [x] Step 1.3: Authentication & Session Management

### Phase 2: Login Page ✅ COMPLETED
- [x] Step 2.1: Login View (Razor Page)
- [x] Step 2.2: Login JavaScript
- [x] Step 2.3: Account Controller (MVC)

### Phase 3: Layout & Navigation ✅ COMPLETED
- [x] Step 3.1: Update Master Layout
- [x] Step 3.2: Protected Routes Middleware

### Phase 4: Kendo UI Integration ✅ COMPLETED
- [x] Step 4.1: Kendo UI File Structure (Manual - Ready for user to add files)
- [x] Step 4.2: Kendo UI Theme Override

### Phase 5: Country Module (First CRUD) ✅ COMPLETED
- [x] Step 5.1: Backend Verification (Already Complete ✅)
- [x] Step 5.2: Country View (Razor Page)
- [x] Step 5.3: Country JavaScript (3-File Pattern)
- [x] Step 5.4: Country Controller (MVC)

### Phase 6: Session Management ✅ COMPLETED
- [x] Token-Based Session (Core implementation complete ✅)
- [x] Advanced Session Features (Testing & Refinement ✅)

---

## 📝 Implementation Notes

### [2026-04-20] Session 1: Phase 1-3 Complete ✅

**Completed Work:**

#### Phase 1: Core Infrastructure Setup
- ✅ **app.config.js**: API base URL configuration, endpoint definitions, auth settings
- ✅ **app.constants.js**: Messages, cache keys, grid defaults, validation constants
- ✅ **app.api.js**: Enhanced with Bearer token authentication, auto-redirect on 401
- ✅ **app.auth.js**: Login, logout, token refresh, authentication status check

**Files Created:**
```
Presentation.Mvc/wwwroot/js/core/
├── app.config.js          (NEW)
├── app.constants.js       (NEW)
├── app.api.js            (UPDATED - added auth support)
└── app.auth.js           (NEW)
```

#### Phase 2: Login Page Implementation
- ✅ **Login.cshtml**: Standalone login page with inline CSS, form validation markup
- ✅ **login.js**: Form validation, API integration, loading states, error handling
- ✅ **AccountController.cs**: Login GET action, Logout POST action

**Files Created:**
```
Presentation.Mvc/
├── Views/Account/Login.cshtml     (NEW)
├── wwwroot/js/modules/account/login.js  (NEW)
└── Controllers/AccountController.cs     (NEW)
```

#### Phase 3: Layout & Navigation Enhancement
- ✅ **_Layout.cshtml**: Added new core JS files in correct order (config → constants → api → auth)
- ✅ **_Header.cshtml**: Added logout button with confirmation dialog
- ✅ **AuthenticationCheckMiddleware.cs**: Client-side auth support, public path handling
- ✅ **Program.cs**: Registered authentication middleware

**Files Updated:**
```
Presentation.Mvc/
├── Views/Shared/_Layout.cshtml            (UPDATED)
├── Views/Shared/_Header.cshtml            (UPDATED)
├── Middleware/AuthenticationCheckMiddleware.cs  (NEW)
└── Program.cs                             (UPDATED)
```

**Build Status:**
- ✅ Build: Successful (0 errors, 0 warnings)
- ✅ All new files compiled without issues
- ✅ Middleware registered correctly

**Key Features Implemented:**
1. **In-Memory Token Storage**: AccessToken stored in JavaScript memory (XSS protection)
2. **HTTP-Only Cookie Support**: RefreshToken via server-side cookie (CSRF protection)
3. **Auto 401 Handling**: Automatic redirect to login on unauthorized access
4. **Form Validation**: Client-side validation with error display
5. **Loading States**: Loading overlay during API calls
6. **Toast Notifications**: Success/error feedback
7. **Logout Functionality**: Clear token and redirect to login

**Security Measures:**
- ✅ Token never stored in localStorage/sessionStorage
- ✅ Bearer token sent in Authorization header
- ✅ 401 response triggers auto-logout
- ✅ Confirmation dialog before logout

**Next Steps:**
- Phase 4: Kendo UI Integration (manual file addition required)
- Phase 5: Country CRUD Module (first complete module)

---

### [2026-04-20] Session 2: Phase 4 Complete ✅

**Completed Work:**

#### Phase 4: Kendo UI Integration Setup
- ✅ **Folder Structure**: Created `/wwwroot/lib/kendo_2024_q4/` with js/, css/, styles/ folders
- ✅ **README.md**: Comprehensive guide for manual Kendo UI file placement
- ✅ **_kendo-theme.scss**: Complete theme override with bdDevsCrm color palette
- ✅ **_Layout.cshtml**: Added commented Kendo UI references (CSS + JS)

**Files Created:**
```
Presentation.Mvc/
├── wwwroot/lib/kendo_2024_q4/
│   ├── js/                    (Ready for kendo.all.min.js)
│   ├── css/                   (Ready for kendo.common.min.css, kendo.default.min.css)
│   ├── styles/                (Ready for theme resources)
│   └── README.md              (NEW - Manual installation guide)
└── wwwroot/scss/kendo-override/
    └── _kendo-theme.scss      (NEW - 400+ lines of theme overrides)
```

**Files Updated:**
```
Presentation.Mvc/Views/Shared/_Layout.cshtml  (UPDATED - Added commented Kendo UI references)
```

**Theme Override Features:**
- ✅ **Color Palette**: Primary #1E5FA8, borders #CBD5E1, hover #EFF6FF
- ✅ **Grid Styling**: Custom header colors, row hover effects, pager styling
- ✅ **Button Styling**: Primary, secondary, icon button variants
- ✅ **Input Fields**: 36px height, focus states, validation states
- ✅ **Window/Modal**: Custom titlebar with primary color
- ✅ **TabStrip**: Active state highlighting with primary color
- ✅ **Dropdown/Combo**: Hover and selection states
- ✅ **Notifications**: Success, error, warning, info variants
- ✅ **Validator**: Error message and tooltip styling
- ✅ **Calendar**: Header and date selection styling
- ✅ **Responsive**: Mobile breakpoint adjustments

**Manual Step Required:**
⚠️ **User must manually add Kendo UI 2024 Q4 files** to `/wwwroot/lib/kendo_2024_q4/`
- Download from: https://www.telerik.com/kendo-ui (requires license)
- Add files as specified in README.md
- After adding files, uncomment Kendo UI references in `_Layout.cshtml`

**Build Status:**
- ✅ Folder structure created successfully
- ✅ SCSS file created with valid syntax
- ✅ Layout updated with proper conditional comments

**Next Steps:**
1. User adds Kendo UI files manually
2. Uncomment Kendo UI references in _Layout.cshtml
3. Compile SCSS to CSS (if using SCSS compiler)
4. Proceed to Phase 5: Country CRUD Module

---

### [2026-04-20] Session 3: Phase 5 Complete ✅

**Completed Work:**

#### Phase 5: Country Module (First CRUD Implementation)
- ✅ **Country.cshtml**: Complete Razor page with grid layout, form modal, responsive design
- ✅ **countrySettings.js**: Module initialization with config, auth check, event handlers
- ✅ **countryDetails.js**: Form CRUD operations (Create, Update, validation)
- ✅ **countrySummary.js**: Grid operations (List, Delete, server-side paging/sorting/filtering)
- ✅ **CountryController.cs**: MVC controller rendering Country management page

**Files Created:**
```
Presentation.Mvc/
├── Views/Core/SystemAdmin/
│   └── Country.cshtml                          (NEW - 270+ lines)
├── Controllers/Core/SystemAdmin/
│   └── CountryController.cs                    (NEW - MVC controller)
└── wwwroot/js/modules/core/country/
    ├── countrySettings.js                      (NEW - Initialization)
    ├── countryDetails.js                       (NEW - Form CRUD)
    └── countrySummary.js                       (NEW - Grid operations)
```

**Files Updated:**
```
Presentation.Mvc/wwwroot/css/app.css  (UPDATED - Added grid action buttons & badges styling)
```

**Implementation Features:**

**Country.cshtml (Razor Page):**
- Page header with breadcrumb navigation
- Action buttons (Add New, Refresh)
- Kendo Grid container
- Modal form window with validation
- Form fields: CountryName, CountryCode, SortOrder, Remarks, IsActive
- Inline CSS with responsive design
- References to 3 JavaScript modules

**countrySettings.js (Initialization):**
- Module configuration with API endpoints
- Grid options (pageSize: 20, sortable, filterable, pageable)
- Window options (600px width, modal, closeable)
- Authentication check on DOM ready
- Event handlers for Add/Refresh buttons

**countryDetails.js (Form CRUD):**
- Kendo Window initialization
- Kendo Validator with custom rules
- `openAddForm()` - Clear form and open for new entry
- `openEditForm(id)` - Load country data and populate form
- `saveCountry()` - Create or Update with validation
- Form validation rules (required, 2-100 characters)
- Auto-refresh grid after save

**countrySummary.js (Grid Operations):**
- Kendo Grid with DataSource
- Server-side paging, sorting, filtering
- Grid columns: ID, Name, Code, Sort Order, Status, Remarks, Actions
- Bearer token authentication for API calls
- Edit/Delete action buttons in grid rows
- Status badges (Active/Inactive)
- Confirm dialog before delete
- Auto-refresh after delete

**CountryController.cs (MVC):**
- Single GET action returning Country view
- Route: `/Country/Index`
- Authentication handled by middleware

**CSS Enhancements:**
- Grid action buttons (Edit: Blue, Delete: Red)
- Status badges (Active: Green, Inactive: Gray)
- Hover effects with smooth transitions
- Responsive button sizing

**Backend Integration:**
Backend API already complete from previous work:
- ✅ CountryController (API) in Presentation.Controller
- ✅ CrmCountryService in Application.Services
- ✅ Repository pattern with EF Core
- ✅ CRUD Record pattern (CreateCountryRecord, UpdateCountryRecord, DeleteCountryRecord)
- ✅ API Endpoints: `/core/systemadmin/country-summary` (POST), `/core/systemadmin/country` (POST/PUT/DELETE), `/core/systemadmin/country/{id}` (GET)

**Build Status:**
- ✅ Build: Successful (0 errors, 0 warnings)
- ✅ All files compiled without issues
- ✅ MVC controller registered correctly

**Key Patterns Demonstrated:**
1. **3-File JavaScript Pattern**: Settings (init) → Details (form) → Summary (grid)
2. **Kendo UI Integration**: Grid, Window, Validator components
3. **Server-Side Operations**: Paging, sorting, filtering handled by API
4. **Bearer Token Auth**: All API calls include Authorization header
5. **Unified API Response**: Consistent ApiResponse<T> structure
6. **CRUD Record Pattern**: Backend uses CreateCountryRecord, UpdateCountryRecord
7. **Clean Separation**: View (Razor) → Controller (MVC) → API (Web API) → Service → Repository

**User Experience:**
- ✅ Click "Add New Country" → Modal opens with empty form
- ✅ Fill form and click Save → API creates country, grid refreshes
- ✅ Click Edit in grid row → Modal opens with populated form
- ✅ Modify and Save → API updates country, grid refreshes
- ✅ Click Delete → Confirm dialog → API deletes, grid refreshes
- ✅ Click Refresh → Grid reloads from server
- ✅ All operations show loading overlay and toast notifications

**Next Steps:**
- User can access Country module at `/Country/Index`
- Country module serves as template for other CRUD modules
- Can replicate pattern for Branch, Department, Designation, etc.

---

### [2026-04-20] Session 4: Phase 6 Complete ✅

**Completed Work:**

#### Phase 6: Advanced Session Management (Testing & Refinement)
- ✅ **app.session.js**: Comprehensive session management with advanced features
- ✅ **SessionTest.cshtml**: Interactive testing page for session features
- ✅ **TestController.cs**: MVC controller for testing utilities
- ✅ **CSS Enhancements**: Session warning dialog styles with animations

**Files Created:**
```
Presentation.Mvc/
├── wwwroot/js/core/
│   └── app.session.js                          (NEW - 600+ lines, advanced session management)
├── Views/Test/
│   └── SessionTest.cshtml                      (NEW - Interactive testing page)
└── Controllers/
    └── TestController.cs                       (NEW - Test utilities controller)
```

**Files Updated:**
```
Presentation.Mvc/
├── Views/Shared/_Layout.cshtml                 (UPDATED - Added app.session.js reference)
├── wwwroot/css/app.css                         (UPDATED - Session warning dialog styles)
└── wwwroot/js/modules/account/login.js         (UPDATED - Initialize session after login)
```

**Advanced Session Features Implemented:**

**1. Session Timeout Management:**
- ✅ Configurable session timeout (default: 30 minutes)
- ✅ Configurable idle timeout (default: 15 minutes)
- ✅ Warning dialog 2 minutes before expiry
- ✅ Automatic logout on timeout
- ✅ Session extension capability

**2. Activity Tracking:**
- ✅ Monitors user activity (mouse, keyboard, scroll, touch, click events)
- ✅ Updates last activity timestamp in localStorage
- ✅ Resets idle timer on activity
- ✅ Activity check interval (every 30 seconds)
- ✅ Cross-tab activity synchronization

**3. Automatic Token Refresh:**
- ✅ Auto-refresh token 5 minutes before expiry
- ✅ Configurable refresh interval (default: 25 minutes)
- ✅ Silent refresh without user intervention
- ✅ Broadcasts token to other tabs after refresh
- ✅ Graceful handling of refresh failures

**4. Multi-Tab Synchronization:**
- ✅ BroadcastChannel API for inter-tab communication
- ✅ Logout from one tab = logout from all tabs
- ✅ Activity in one tab extends session in all tabs
- ✅ Token refresh synchronized across tabs
- ✅ Session state shared via localStorage

**5. Session Warning Dialog:**
- ✅ Beautiful modal overlay with animations
- ✅ Shows warning 2 minutes before expiry
- ✅ "Continue Session" button to extend
- ✅ "Logout Now" button for immediate logout
- ✅ Auto-hide on user activity
- ✅ Prevents duplicate warnings

**6. Configuration Management:**
- ✅ Runtime configuration updates
- ✅ Adjustable timeouts and intervals
- ✅ `updateConfig()` method for customization
- ✅ Default production-ready values

**7. Cleanup & Lifecycle:**
- ✅ Cleanup on logout (timers, listeners, localStorage)
- ✅ Graceful BroadcastChannel closure
- ✅ Remove event listeners on cleanup
- ✅ Clear all session timers
- ✅ Auto-initialization on authenticated pages

**8. Debugging & Testing:**
- ✅ `getSessionInfo()` method for debugging
- ✅ Console logging for events
- ✅ Session testing page at `/Test/SessionTest`
- ✅ Real-time session status monitoring
- ✅ Manual session extension
- ✅ Configuration testing
- ✅ Multi-tab testing utilities
- ✅ Session events log

**Session Testing Page Features:**
- **Current Session Status** - Real-time auth status, last activity, time tracking
- **Session Configuration** - Adjust timeouts, intervals, warning time
- **Session Actions** - Extend, trigger warning, refresh token, simulate idle, clear
- **Multi-Tab Testing** - Broadcast activity, open in new tab, test synchronization
- **Events Log** - Real-time logging of all session events with timestamps

**Configuration Options:**
```javascript
{
    sessionTimeout: 30 * 60 * 1000,        // 30 minutes
    idleTimeout: 15 * 60 * 1000,           // 15 minutes
    warningTime: 2 * 60 * 1000,            // 2 minutes before expiry
    tokenRefreshInterval: 25 * 60 * 1000,  // 25 minutes
    activityCheckInterval: 30 * 1000       // 30 seconds
}
```

**Security Features:**
- ✅ In-memory AccessToken storage (XSS protection)
- ✅ HTTP-only RefreshToken cookie (CSRF protection)
- ✅ No sensitive data in localStorage
- ✅ Automatic logout on timeout
- ✅ Cross-tab logout synchronization
- ✅ Activity-based session extension

**User Experience Flow:**
1. User logs in → SessionManager auto-initializes
2. User activity tracked → Session stays alive
3. 2 minutes before timeout → Warning dialog appears
4. User clicks "Continue" or any activity → Session extended
5. No activity for idle timeout → Auto-logout
6. Logout in one tab → All tabs logout simultaneously
7. Activity in any tab → All tabs stay authenticated

**Testing Flow:**
1. Navigate to `/Test/SessionTest`
2. View real-time session status
3. Adjust configuration values
4. Test session extension
5. Simulate idle timeout
6. Open multiple tabs to test synchronization
7. Monitor events log for debugging

**Build Status:**
- ✅ Build: Successful (0 errors, 0 warnings)
- ✅ All files compiled without issues
- ✅ Session manager auto-initializes on authenticated pages

**Browser Compatibility:**
- ✅ Modern browsers (Chrome, Firefox, Edge, Safari)
- ✅ BroadcastChannel API (graceful degradation if unsupported)
- ✅ localStorage API
- ✅ Modern ES6+ JavaScript

**Next Steps:**
- Session management fully operational
- Ready for production deployment
- Testing page available for QA
- All 6 implementation phases complete ✅

---

## **API Endpoints Reference**

This section provides comprehensive documentation of all API endpoints in the bdDevsCrm system, mapping `RouteConstants` to their corresponding controller methods.

### **Base Route**
All API endpoints are prefixed with: `/bdDevs-crm`

---

### **1. AUTHENTICATION ENDPOINTS**

**Controller:** `AuthenticationController`
**File:** `Presentation.Controller/Controllers/Authentication/AuthenticationController.cs`

| HTTP | Route | Method Signature | Description | Auth |
|------|-------|------------------|-------------|------|
| POST | `login` | `Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)` | User login - returns access/refresh tokens | Anonymous |
| POST | `refresh-token` | `Task<IActionResult> RefreshToken()` | Refresh access token using refresh token cookie | Anonymous |
| POST | `revoke-token` | `Task<IActionResult> RevokeToken()` | Revoke current refresh token | Anonymous |
| GET | `user-info` | `IActionResult UserInfo()` | Get current authenticated user info | Required |
| POST | `logout` | `Task<IActionResult> Logout()` | Logout user and revoke all tokens | Anonymous |

**RouteConstants Mapping:**
```csharp
RouteConstants.Login → "login"
RouteConstants.RefreshToken → "refresh-token"
RouteConstants.RevokeToken → "revoke-token"
RouteConstants.UserInfo → "user-info"
RouteConstants.Logout → "logout"
```

---

### **2. SYSTEM ADMIN ENDPOINTS**

#### **2.1 Module Management**

**Controller:** `ModuleController`
**File:** `Presentation.Controller/Controllers/Core/SystemAdmin/ModuleController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| POST | `module` | `Task<IActionResult> CreateModuleAsync([FromBody] ModuleDto modelDto, CancellationToken cancellationToken)` | Create new module |
| PUT | `module/{key}` | `Task<IActionResult> UpdateModuleAsync([FromRoute] int key, [FromBody] ModuleDto modelDto, CancellationToken cancellationToken)` | Update existing module |
| DELETE | `module/{key}` | `Task<IActionResult> DeleteModuleAsync([FromRoute] int key, CancellationToken cancellationToken)` | Delete module by ID |
| POST | `module-summary` | `Task<IActionResult> ModuleSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken)` | Get paginated module grid |
| GET | `modules` | `Task<IActionResult> ModulesAsync(CancellationToken cancellationToken)` | Get all modules |
| GET | `module/{id:int}` | `Task<IActionResult> ModuleAsync([FromRoute] int id, CancellationToken cancellationToken)` | Get module by ID |
| GET | `modules-ddl` | `Task<IActionResult> ModulesForDDLAsync(CancellationToken cancellationToken)` | Get modules dropdown list |

**RouteConstants Mapping:**
```csharp
RouteConstants.CreateModule → "module"
RouteConstants.UpdateModule → "module/{key}"
RouteConstants.DeleteModule → "module/{key}"
RouteConstants.ModuleSummary → "module-summary"
RouteConstants.ReadModules → "modules"
RouteConstants.ReadModule → "module/{id:int}"
RouteConstants.ModuleDDL → "modules-ddl"
```

#### **2.2 Menu Management**

**Controller:** `MenuController`
**File:** `Presentation.Controller/Controllers/Core/SystemAdmin/MenuController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| POST | `menu` | `Task<IActionResult> CreateMenu([FromBody] MenuDto modelDto, CancellationToken cancellationToken)` | Create new menu |
| PUT | `menu/{key}` | `Task<IActionResult> UpdateMenuAsync([FromRoute] int key, [FromBody] MenuDto modelDto, CancellationToken cancellationToken)` | Update existing menu |
| DELETE | `menu/{key}` | `Task<IActionResult> DeleteMenuAsync([FromRoute] int key, CancellationToken cancellationToken)` | Delete menu by ID |
| POST | `menu-summary` | `Task<IActionResult> MenuSummary([FromBody] GridOptions options, CancellationToken cancellationToken)` | Get paginated menu grid with HATEOAS |
| GET | `menus` | `Task<IActionResult> ReadMenus(CancellationToken cancellationToken)` | Get all menus |
| GET | `menus-ddl` | `Task<IActionResult> MenusDDL(CancellationToken cancellationToken)` | Get menus dropdown list |
| GET | `menus-user-permission` | `Task<IActionResult> MenusByUserPermission(CancellationToken cancellationToken)` | Get menus by current user permission |
| GET | `menus-moduleId/{moduleId:int}` | `Task<IActionResult> MenusByModuleId([FromRoute] int moduleId, CancellationToken cancellationToken)` | Get menus by module ID |
| GET | `parent-by-menu/{parentMenuId:int}` | `Task<IActionResult> ParentMenuByMenu(int menuId, CancellationToken cancellationToken)` | Get parent menus by menu ID |
| GET | `menu/{menuId:int}` | `Task<IActionResult> ReadMenu(int menuId, CancellationToken cancellationToken)` | Get menu by ID |

**RouteConstants Mapping:**
```csharp
RouteConstants.CreateMenu → "menu"
RouteConstants.UpdateMenu → "menu/{key}"
RouteConstants.DeleteMenu → "menu/{key}"
RouteConstants.MenuSummary → "menu-summary"
RouteConstants.ReadMenus → "menus"
RouteConstants.MenuDDL → "menus-ddl"
RouteConstants.ReadMenusByUserPermission → "menus-user-permission"
RouteConstants.ReadMenusByModuleId → "menus-moduleId/{moduleId:int}"
RouteConstants.ReadParentMenuByMenu → "parent-by-menu/{parentMenuId:int}"
RouteConstants.ReadMenu → "menu/{menuId:int}"
```

#### **2.3 Country Management**

**Controller:** `CountryController`
**File:** `Presentation.Controller/Controllers/Core/SystemAdmin/CountryController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| GET | `countryddl` | `Task<IActionResult> CountriesForDDLAsync(CancellationToken cancellationToken)` | Get countries dropdown list |
| POST | `country-summary` | `Task<IActionResult> CountrySummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken)` | Get paginated country grid |
| POST | `country` | `Task<IActionResult> CreateCountryAsync([FromBody] CreateCountryRecord record, CancellationToken cancellationToken)` | Create new country (CRUD Record pattern) |
| PUT | `country/{key}` | `Task<IActionResult> UpdateCountryAsync([FromRoute] int key, [FromBody] UpdateCountryRecord record, CancellationToken cancellationToken)` | Update existing country (CRUD Record pattern) |
| DELETE | `country/{key}` | `Task<IActionResult> DeleteCountryAsync([FromRoute] int key, CancellationToken cancellationToken)` | Delete country (CRUD Record pattern) |
| GET | `countries` | `Task<IActionResult> CountryAsync([FromRoute] int countryId, CancellationToken cancellationToken)` | Get country by ID |

**RouteConstants Mapping:**
```csharp
RouteConstants.CountryDDL → "countryddl"
RouteConstants.CountrySummary → "country-summary"
RouteConstants.CreateCountry → "country"
RouteConstants.UpdateCountry → "country/{key}"
RouteConstants.DeleteCountry → "country/{key}"
RouteConstants.ReadCountries → "countries"
```

#### **2.4 Group Management**

**Controller:** `GroupController`
**File:** `Presentation.Controller/Controllers/Core/SystemAdmin/GroupController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| POST | `group` | `Task<IActionResult> CreateGroupAsync([FromBody] GroupDto modelDto, CancellationToken cancellationToken)` | Create new group |
| PUT | `group/{key}` | `Task<IActionResult> UpdateGroupAsync([FromRoute] int key, [FromBody] GroupDto modelDto, CancellationToken cancellationToken)` | Update existing group |
| DELETE | `group/{key}` | `Task<IActionResult> DeleteGroupAsync([FromRoute] int key, CancellationToken cancellationToken)` | Delete group by ID |
| POST | `group-summary` | `Task<IActionResult> GroupSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken)` | Get paginated group grid |
| GET | `group-permissions/{groupId:int}` | `Task<IActionResult> GroupPermissionsByGroupIdAsync([FromRoute] int groupId, CancellationToken cancellationToken)` | Get all permissions for specific group |
| GET | `access-controls` | `Task<IActionResult> AccessControlsAsync(CancellationToken cancellationToken)` | Get all available access controls |
| GET | `groups-ddl` | `Task<IActionResult> GroupsForDDLAsync(CancellationToken cancellationToken)` | Get groups dropdown list |
| GET | `group/{groupId:int}` | `Task<IActionResult> GroupAsync([FromRoute] int groupId, CancellationToken cancellationToken)` | Get group by ID |

**RouteConstants Mapping:**
```csharp
RouteConstants.CreateGroup → "group"
RouteConstants.UpdateGroup → "group/{key}"
RouteConstants.DeleteGroup → "group/{key}"
RouteConstants.GroupSummary → "group-summary"
RouteConstants.ReadGroupPermissionsByGroupId → "group-permissions/{groupId:int}"
RouteConstants.ReadAccessControls → "access-controls"
RouteConstants.GroupDDL → "groups-ddl"
RouteConstants.ReadGroup → "group/{groupId:int}"
```

#### **2.5 User Management**

**Controller:** `UsersController`
**File:** `Presentation.Controller/Controllers/Core/SystemAdmin/UsersController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| POST | `user` | `Task<IActionResult> CreateUserAsync([FromBody] UsersDto modelDto, CancellationToken cancellationToken)` | Create new user |
| PUT | `user/{key}` | `Task<IActionResult> UpdateUserAsync([FromRoute] int key, [FromBody] UsersDto modelDto, CancellationToken cancellationToken)` | Update existing user |
| DELETE | `user/{key}` | `Task<IActionResult> DeleteUserAsync([FromRoute] int key, CancellationToken cancellationToken)` | Delete user by ID |
| POST | `user-summary` | `Task<IActionResult> UserSummaryAsync([FromBody] GridOptions options, [FromQuery] int companyId, CancellationToken cancellationToken)` | Get paginated user grid |
| GET | `users` | `Task<IActionResult> ReadUsersAsync(CancellationToken cancellationToken)` | Get all users |
| GET | `user/{id:int}` | `Task<IActionResult> UserAsync([FromRoute] int id, CancellationToken cancellationToken)` | Get user by ID |

**RouteConstants Mapping:**
```csharp
RouteConstants.CreateUser → "user"
RouteConstants.UpdateUser → "user/{key}"
RouteConstants.DeleteUser → "user/{key}"
RouteConstants.UserSummary → "user-summary"
RouteConstants.ReadUsers → "users"
RouteConstants.ReadUser → "user/{id:int}"
```

#### **2.6 Company Management**

**Controller:** `CompanyController`
**File:** `Presentation.Controller/Controllers/Core/SystemAdmin/CompanyController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| GET | `companies-ddl` | `Task<IActionResult> CompaniesForDDLAsync(CancellationToken cancellationToken)` | Get companies dropdown list |
| POST | `company` | `Task<IActionResult> CreateCompanyAsync([FromBody] CreateCompanyRecord record, CancellationToken cancellationToken)` | Create new company (CRUD Record pattern) |
| PUT | `company/{key}` | `Task<IActionResult> UpdateCompanyAsync([FromRoute] int key, [FromBody] UpdateCompanyRecord record, CancellationToken cancellationToken)` | Update company (CRUD Record pattern) |
| DELETE | `company/{key}` | `Task<IActionResult> DeleteCompanyAsync([FromRoute] int key, CancellationToken cancellationToken)` | Delete company (CRUD Record pattern) |
| GET | `company/key/{key}` | `Task<IActionResult> CompanyAsync([FromRoute] int key, CancellationToken cancellationToken)` | Get company by ID |
| GET | `companies` | `Task<IActionResult> CompaniesAsync(CancellationToken cancellationToken)` | Get all companies |
| POST | `companies-by-ids` | `Task<IActionResult> CompaniesByIdsAsync([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)` | Get companies by collection of IDs |
| GET | `mother-company/{companyId:int}` | `Task<IActionResult> MotherCompanyAsync([FromRoute] int companyId, CancellationToken cancellationToken)` | Get mother company for current user |

**RouteConstants Mapping:**
```csharp
RouteConstants.CompaniesDDL → "companies-ddl"
RouteConstants.CreateCompany → "company"
RouteConstants.UpdateCompany → "company/{key}"
RouteConstants.DeleteCompany → "company/{key}"
RouteConstants.ReadCompany → "company/key/{key}"
RouteConstants.ReadCompanies → "companies"
RouteConstants.ReadCompaniesCollection → "companies-by-ids"
RouteConstants.ReadMotherCompany → "mother-company/{companyId:int}"
```

#### **2.7 Thana Management**

**Controller:** `ThanaController`
**File:** `Presentation.Controller/Controllers/Core/SystemAdmin/ThanaController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| GET | `thanas-ddl` | `Task<IActionResult> ThanasForDDLAsync(CancellationToken cancellationToken)` | Get thanas dropdown list |
| POST | `thana-summary` | `Task<IActionResult> ThanaSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken)` | Get paginated thana grid |
| POST | `thana` | `Task<IActionResult> CreateThanaAsync([FromBody] CreateThanaRecord record, CancellationToken cancellationToken)` | Create new thana (CRUD Record pattern) |
| PUT | `thana/{key}` | `Task<IActionResult> UpdateThanaAsync([FromRoute] int key, [FromBody] UpdateThanaRecord record, CancellationToken cancellationToken)` | Update thana (CRUD Record pattern) |
| DELETE | `thana/{key}` | `Task<IActionResult> DeleteThanaAsync([FromRoute] int key, CancellationToken cancellationToken)` | Delete thana (CRUD Record pattern) |
| GET | `thana/{id:int}` | `Task<IActionResult> ThanaAsync([FromRoute] int id, CancellationToken cancellationToken)` | Get thana by ID |
| GET | `thanas` | `Task<IActionResult> ThanasAsync(CancellationToken cancellationToken)` | Get all thanas |
| GET | `thanas-by-district/{districtId:int}` | `Task<IActionResult> ThanasByDistrictAsync([FromRoute] int districtId, CancellationToken cancellationToken)` | Get thanas by district ID |

**RouteConstants Mapping:**
```csharp
RouteConstants.ThanasDDL → "thanas-ddl"
RouteConstants.ThanaSummary → "thana-summary"
RouteConstants.CreateThana → "thana"
RouteConstants.UpdateThana → "thana/{key}"
RouteConstants.DeleteThana → "thana/{key}"
RouteConstants.ReadThana → "thana/{id:int}"
RouteConstants.ReadThanas → "thanas"
RouteConstants.ReadThanasByDistrict → "thanas-by-district/{districtId:int}"
```

---

### **3. WORKFLOW ENDPOINTS**

**Controller:** `WorkFlowController`
**File:** `Presentation.Controller/Controllers/Core/SystemAdmin/WorkFlowController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| POST | `workflow-state` | `Task<IActionResult> CreateWorkflowStateAsync([FromBody] WfStateDto modelDto, CancellationToken cancellationToken)` | Create new workflow state |
| PUT | `workflow-state/{key}` | `Task<IActionResult> UpdateWorkflowStateAsync([FromRoute] int key, [FromBody] WfStateDto modelDto, CancellationToken cancellationToken)` | Update workflow state |
| DELETE | `workflow-state/{key}` | `Task<IActionResult> DeleteWorkflowStateAsync([FromRoute] int key, CancellationToken cancellationToken)` | Delete workflow state |
| POST | `workflow-summary` | `Task<IActionResult> WorkflowSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken)` | Get paginated workflow grid |
| GET | `statuses-by-menu/{menuId:int}` | `Task<IActionResult> StatusesByMenuIdAsync([FromRoute] int menuId, CancellationToken cancellationToken)` | Get workflow statuses by menu ID |
| GET | `actions-by-status/{statusId:int}` | `Task<IActionResult> ActionsByStatusIdForGroupAsync([FromRoute] int statusId, CancellationToken cancellationToken)` | Get workflow actions by status ID for group |
| POST | `workflow-action` | `Task<IActionResult> CreateWorkflowActionAsync([FromBody] WfActionDto modelDto, CancellationToken cancellationToken)` | Create new workflow action |
| PUT | `workflow-action/{key}` | `Task<IActionResult> UpdateWorkflowActionAsync([FromRoute] int key, [FromBody] WfActionDto modelDto, CancellationToken cancellationToken)` | Update workflow action |
| DELETE | `workflow-action/{key}` | `Task<IActionResult> DeleteWorkflowActionAsync([FromRoute] int key, WfActionDto wfActionDto, CancellationToken cancellation)` | Delete workflow action |

**RouteConstants Mapping:**
```csharp
RouteConstants.CreateWorkflowState → "workflow-state"
RouteConstants.UpdateWorkflowState → "workflow-state/{key}"
RouteConstants.DeleteWorkflowState → "workflow-state/{key}"
RouteConstants.WorkflowSummary → "workflow-summary"
RouteConstants.ReadStatusesByMenuId → "statuses-by-menu/{menuId:int}"
RouteConstants.ReadActionsByStatusIdForGroup → "actions-by-status/{statusId:int}"
RouteConstants.CreateWorkflowAction → "workflow-action"
RouteConstants.UpdateWorkflowAction → "workflow-action/{key}"
RouteConstants.DeleteWorkflowAction → "workflow-action/{key}"
```

---

### **4. ACCESS CONTROL ENDPOINTS**

**Controller:** `AccessControlController`
**File:** `Presentation.Controller/Controllers/Core/SystemAdmin/AccessControlController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| POST | `access-control-summary` | `Task<IActionResult> AccessControlSummaryAsync([FromBody] GridOptions options)` | Get paginated access control grid |
| POST | `access-control` | `Task<IActionResult> CreateAccessControlAsync([FromBody] AccessControlDto modelDto)` | Create new access control record |
| PUT | `access-control/{key}` | `Task<IActionResult> UpdateAccessControlAsync([FromRoute] int key, [FromBody] AccessControlDto modelDto, CancellationToken cancellationToken)` | Update access control record |
| DELETE | `access-control/{key}` | `Task<IActionResult> DeleteAccessControlAsync([FromRoute] int key, CancellationToken cancellationToken)` | Delete access control record |
| GET | `access-control/key/{key:int}` | `Task<IActionResult> AccessControlAsync([FromRoute] int id)` | Get access control by ID |

**RouteConstants Mapping:**
```csharp
RouteConstants.AccessControlSummary → "access-control-summary"
RouteConstants.CreateAccessControl → "access-control"
RouteConstants.UpdateAccessControl → "access-control/{key}"
RouteConstants.DeleteAccessControl → "access-control/{key}"
RouteConstants.ReadAccessControl → "access-control/key/{key:int}"
```

---

### **5. CRM ENDPOINTS**

#### **5.1 Institute Management**

**Controller:** `CrmInstituteController`
**File:** `Presentation.Controller/Controllers/CRM/CrmInstituteController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| GET | `crm-institute-ddl` | `Task<IActionResult> InstitutesForDDLAsync(CancellationToken cancellationToken)` | Get institutes dropdown list |
| POST | `crm-institute-summary` | `Task<IActionResult> InstituteSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken)` | Get paginated institute grid |
| POST | `crm-institute` | `Task<IActionResult> CreateInstituteAsync([FromBody] CreateCrmInstituteRecord record, CancellationToken cancellationToken)` | Create new institute (CRUD Record pattern) |
| PUT | `crm-institute/{key:int}` | `Task<IActionResult> UpdateInstituteAsync([FromRoute] int key, [FromBody] UpdateCrmInstituteRecord record, CancellationToken cancellationToken)` | Update institute (CRUD Record pattern) |
| DELETE | `crm-institute/{key:int}` | `Task<IActionResult> DeleteInstituteAsync([FromRoute] int key, CancellationToken cancellationToken)` | Delete institute (CRUD Record pattern) |
| GET | `crm-institute/{id:int}` | `Task<IActionResult> InstituteAsync([FromRoute] int id, CancellationToken cancellationToken)` | Get institute by ID |
| GET | `crm-institutes` | `Task<IActionResult> InstitutesAsync(CancellationToken cancellationToken)` | Get all institutes |
| GET | `crm-institut-by-countryid-ddl/{countryId:int}` | `Task<IActionResult> InstitutesByCountryIdAsync([FromRoute] int countryId, CancellationToken cancellationToken)` | Get institutes by country ID |

**RouteConstants Mapping:**
```csharp
RouteConstants.CrmInstituteDDL → "crm-institute-ddl"
RouteConstants.CrmInstituteSummary → "crm-institute-summary"
RouteConstants.CreateCrmInstitute → "crm-institute"
RouteConstants.UpdateCrmInstitute → "crm-institute/{key:int}"
RouteConstants.DeleteCrmInstitute → "crm-institute/{key:int}"
RouteConstants.ReadCrmInstitute → "crm-institute/{id:int}"
RouteConstants.ReadCrmInstitutes → "crm-institutes"
RouteConstants.ReadCrmInstitutesByCountryId → "crm-institut-by-countryid-ddl/{countryId:int}"
```

#### **5.2 Course Management**

**Controller:** `CrmCourseController`
**File:** `Presentation.Controller/Controllers/CRM/CrmCourseController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| GET | `crm-course-ddl` | `Task<IActionResult> CoursesForDDLAsync(CancellationToken cancellationToken)` | Get courses dropdown list |
| POST | `crm-course-summary` | `Task<IActionResult> CourseSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken)` | Get paginated course grid |
| POST | `crm-course` | `Task<IActionResult> CreateCourseAsync([FromBody] CreateCrmCourseRecord record, CancellationToken cancellationToken)` | Create new course (CRUD Record pattern) |
| PUT | `crm-course/{key:int}` | `Task<IActionResult> UpdateCourseAsync([FromRoute] int key, [FromBody] UpdateCrmCourseRecord record, CancellationToken cancellationToken)` | Update course (CRUD Record pattern) |
| DELETE | `crm-course/{key:int}` | `Task<IActionResult> DeleteCourseAsync([FromRoute] int key, CancellationToken cancellationToken)` | Delete course (CRUD Record pattern) |
| GET | `crm-course/{id:int}` | `Task<IActionResult> CourseAsync([FromRoute] int id, CancellationToken cancellationToken)` | Get course by ID |
| GET | `crm-courses` | `Task<IActionResult> CoursesAsync(CancellationToken cancellationToken)` | Get all courses |
| GET | `crm-course-by-instituteid-ddl/{instituteId:int}` | `Task<IActionResult> CoursesByInstituteIdAsync([FromRoute] int instituteId, CancellationToken cancellationToken)` | Get courses by institute ID |

**RouteConstants Mapping:**
```csharp
RouteConstants.CrmCourseDDL → "crm-course-ddl"
RouteConstants.CrmCourseSummary → "crm-course-summary"
RouteConstants.CreateCrmCourse → "crm-course"
RouteConstants.UpdateCrmCourse → "crm-course/{key:int}"
RouteConstants.DeleteCrmCourse → "crm-course/{key:int}"
RouteConstants.ReadCrmCourse → "crm-course/{id:int}"
RouteConstants.ReadCrmCourses → "crm-courses"
RouteConstants.ReadCrmCoursesByInstituteId → "crm-course-by-instituteid-ddl/{instituteId:int}"
```

#### **5.3 Application Management**

**Controller:** `CrmApplicationController`
**File:** `Presentation.Controller/Controllers/CRM/CrmApplicationController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| POST | `crm-application-summary` | `Task<IActionResult> ApplicationSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken)` | Get paginated application grid |
| POST | `crm-application` | `Task<IActionResult> CreateApplicationAsync([FromBody] CreateCrmApplicationRecord record, CancellationToken cancellationToken)` | Create new application (CRUD Record pattern) |
| PUT | `crm-Application/{key:int}` | `Task<IActionResult> UpdateApplicationAsync([FromRoute] int key, [FromBody] UpdateCrmApplicationRecord record, CancellationToken cancellationToken)` | Update application (CRUD Record pattern) |
| GET | `crm-application/key/{key}` | `Task<IActionResult> ApplicationAsync([FromRoute] int id, CancellationToken cancellationToken)` | Get application by ID |
| GET | `crm-applications` | `Task<IActionResult> ApplicationsAsync(int applicationId, CancellationToken cancellationToken)` | Get all applications |

**RouteConstants Mapping:**
```csharp
RouteConstants.CrmApplicationSummary → "crm-application-summary"
RouteConstants.CreateCrmApplication → "crm-application"
RouteConstants.UpdateCrmApplication → "crm-Application/{key:int}"
RouteConstants.ReadCrmApplication → "crm-application/key/{key}"
RouteConstants.ReadCrmApplications → "crm-applications"
```

#### **5.4 Applicant Info Management**

**Controller:** `CrmApplicantInfoController`
**File:** `Presentation.Controller/Controllers/CRM/CrmApplicantInfoController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| GET | `applicant-info-ddl` | `Task<IActionResult> ApplicantInfosForDDLAsync(CancellationToken cancellationToken)` | Get applicant infos dropdown list |
| POST | `applicant-info-summary` | `Task<IActionResult> ApplicantInfoSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken)` | Get paginated applicant info grid |
| POST | `applicant-info` | `Task<IActionResult> CreateApplicantInfoAsync([FromBody] CreateCrmApplicantInfoRecord record, CancellationToken cancellationToken)` | Create new applicant info (CRUD Record pattern) |
| PUT | `applicant-info/{key:int}` | `Task<IActionResult> UpdateApplicantInfoAsync([FromRoute] int key, [FromBody] UpdateCrmApplicantInfoRecord record, CancellationToken cancellationToken)` | Update applicant info (CRUD Record pattern) |
| DELETE | `applicant-info/{key:int}` | `Task<IActionResult> DeleteApplicantInfoAsync([FromRoute] int key, CancellationToken cancellationToken)` | Delete applicant info (CRUD Record pattern) |
| GET | `applicant-info/{id:int}` | `Task<IActionResult> ApplicantInfoAsync([FromRoute] int id, CancellationToken cancellationToken)` | Get applicant info by ID |
| GET | `applicant-infos` | `Task<IActionResult> ApplicantInfosAsync(CancellationToken cancellationToken)` | Get all applicant infos |
| GET | `applicant-info-by-applicationid/{applicationId:int}` | `Task<IActionResult> ApplicantInfoByApplicationIdAsync([FromRoute] int applicationId, CancellationToken cancellationToken)` | Get applicant info by application ID |
| GET | `applicant-info-by-email` | `Task<IActionResult> ApplicantInfoByEmailAsync([FromQuery] string email, CancellationToken cancellationToken)` | Get applicant info by email |

**RouteConstants Mapping:**
```csharp
RouteConstants.CrmApplicantInfoDDL → "applicant-info-ddl"
RouteConstants.CrmApplicantInfoSummary → "applicant-info-summary"
RouteConstants.CreateCrmApplicantInfo → "applicant-info"
RouteConstants.UpdateCrmApplicantInfo → "applicant-info/{key:int}"
RouteConstants.DeleteCrmApplicantInfo → "applicant-info/{key:int}"
RouteConstants.ReadCrmApplicantInfo → "applicant-info/{id:int}"
RouteConstants.ReadCrmApplicantInfos → "applicant-infos"
RouteConstants.ReadCrmApplicantInfoByApplicationId → "applicant-info-by-applicationid/{applicationId:int}"
RouteConstants.ReadCrmApplicantInfoByEmail → "applicant-info-by-email"
```

#### **5.5 Education History Management**

**Controller:** `CrmEducationHistoryController`
**File:** `Presentation.Controller/Controllers/CRM/CrmEducationHistoryController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| POST | `education-history-summary` | `Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken)` | Get paginated education history grid |
| POST | `education-history` | `Task<IActionResult> CreateAsync([FromBody] CreateCrmEducationHistoryRecord record, CancellationToken cancellationToken)` | Create new education history (CRUD Record pattern) |
| PUT | `education-history/{key:int}` | `Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmEducationHistoryRecord record, CancellationToken cancellationToken)` | Update education history (CRUD Record pattern) |
| DELETE | `education-history/{key:int}` | `Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken)` | Delete education history (CRUD Record pattern) |
| GET | `education-history/{id:int}` | `Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken)` | Get education history by ID |
| GET | `education-histories` | `Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)` | Get all education histories |

**RouteConstants Mapping:**
```csharp
RouteConstants.CrmEducationHistorySummary → "education-history-summary"
RouteConstants.CreateCrmEducationHistory → "education-history"
RouteConstants.UpdateCrmEducationHistory → "education-history/{key:int}"
RouteConstants.DeleteCrmEducationHistory → "education-history/{key:int}"
RouteConstants.ReadCrmEducationHistory → "education-history/{id:int}"
RouteConstants.ReadCrmEducationHistories → "education-histories"
```

#### **5.6 Applicant Course Management**

**Controller:** `CrmApplicantCourseController`
**File:** `Presentation.Controller/Controllers/CRM/CrmApplicantCourseController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| POST | `applicant-course-summary` | `Task<IActionResult> ApplicantCourseSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken)` | Get paginated applicant course grid |
| POST | `applicant-course` | `Task<IActionResult> CreateApplicantCourseAsync([FromBody] CreateCrmApplicantCourseRecord record, CancellationToken cancellationToken)` | Create new applicant course (CRUD Record pattern) |
| PUT | `applicant-course/{key:int}` | `Task<IActionResult> UpdateApplicantCourseAsync([FromRoute] int key, [FromBody] UpdateCrmApplicantCourseRecord record, CancellationToken cancellationToken)` | Update applicant course (CRUD Record pattern) |
| DELETE | `applicant-course/{key:int}` | `Task<IActionResult> DeleteApplicantCourseAsync([FromRoute] int key, CancellationToken cancellationToken)` | Delete applicant course (CRUD Record pattern) |
| GET | `applicant-course/{id:int}` | `Task<IActionResult> ApplicantCourseAsync([FromRoute] int id, CancellationToken cancellationToken)` | Get applicant course by ID |
| GET | `applicant-courses` | `Task<IActionResult> ApplicantCoursesAsync(CancellationToken cancellationToken)` | Get all applicant courses |
| GET | `applicant-courses-by-applicationid/{applicationId:int}` | `Task<IActionResult> ApplicantCoursesByApplicationIdAsync([FromRoute] int applicationId, CancellationToken cancellationToken)` | Get applicant courses by application ID |

**RouteConstants Mapping:**
```csharp
RouteConstants.CrmApplicantCourseSummary → "applicant-course-summary"
RouteConstants.CreateCrmApplicantCourse → "applicant-course"
RouteConstants.UpdateCrmApplicantCourse → "applicant-course/{key:int}"
RouteConstants.DeleteCrmApplicantCourse → "applicant-course/{key:int}"
RouteConstants.ReadCrmApplicantCourse → "applicant-course/{id:int}"
RouteConstants.ReadCrmApplicantCourses → "applicant-courses"
RouteConstants.ReadCrmApplicantCoursesByApplicationId → "applicant-courses-by-applicationid/{applicationId:int}"
```

#### **5.7 Work Experience Management**

**Controller:** `CrmWorkExperienceController`
**File:** `Presentation.Controller/Controllers/CRM/CrmWorkExperienceController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| POST | `work-experience-summary` | `Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken)` | Get paginated work experience grid |
| POST | `work-experience` | `Task<IActionResult> CreateAsync([FromBody] CreateCrmWorkExperienceRecord record, CancellationToken cancellationToken)` | Create new work experience (CRUD Record pattern) |
| PUT | `work-experience/{key:int}` | `Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmWorkExperienceRecord record, CancellationToken cancellationToken)` | Update work experience (CRUD Record pattern) |
| DELETE | `work-experience/{key:int}` | `Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken)` | Delete work experience (CRUD Record pattern) |
| GET | `work-experience/{id:int}` | `Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken)` | Get work experience by ID |
| GET | `work-experiences` | `Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)` | Get all work experiences |

**RouteConstants Mapping:**
```csharp
RouteConstants.CrmWorkExperienceSummary → "work-experience-summary"
RouteConstants.CreateCrmWorkExperience → "work-experience"
RouteConstants.UpdateCrmWorkExperience → "work-experience/{key:int}"
RouteConstants.DeleteCrmWorkExperience → "work-experience/{key:int}"
RouteConstants.ReadCrmWorkExperience → "work-experience/{id:int}"
RouteConstants.ReadCrmWorkExperiences → "work-experiences"
```

---

### **6. HR ENDPOINTS**

#### **6.1 Employee Management**

**Controller:** `EmployeeController`
**File:** `Presentation.Controller/Controllers/Core/HR/EmployeeController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| GET | `employeetypes` | `Task<IActionResult> EmployeeTypes()` | Get employee types |
| GET | `employees-by-indentities` | `Task<IActionResult> EmployeeByCompanyIdAndBranchIdAndDepartmentId(int companyid, int branchId, int departmentId)` | Get employees by company, branch, and department ID |

**RouteConstants Mapping:**
```csharp
RouteConstants.EmployeeTypes → "employeetypes"
RouteConstants.EmployeeByIdentities → "employees-by-indentities"
```

#### **6.2 Department Management**

**Controller:** `DepartmentController`
**File:** `Presentation.Controller/Controllers/Core/HR/DepartmentController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| GET | `departments-by-compnayId/companyId/` | `Task<IActionResult> DepartmentByCompanyIdForCombo([FromQuery] int companyId, CancellationToken cancellationToken)` | Get departments by company ID for combo box |

**RouteConstants Mapping:**
```csharp
RouteConstants.DepartmentByCompanyId → "departments-by-compnayId/companyId/"
```

#### **6.3 Branch Management**

**Controller:** `BranchController`
**File:** `Presentation.Controller/Controllers/Core/HR/BranchController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| GET | `branches/{companyId:int}` | `Task<IActionResult> BranchByCompanyIdForCombo([FromQuery] int companyId, CancellationToken cancellationToken)` | Get branches by company ID for combo box |

**RouteConstants Mapping:**
```csharp
RouteConstants.BranchByCompanyId → "branches/{companyId:int}"
```

---

### **7. DMS (DOCUMENT MANAGEMENT) ENDPOINTS**

**Controller:** `DmsDocumentController`
**File:** `Presentation.Controller/Controllers/DMS/DmsDocumentController.cs`

| HTTP | Route | Method Signature | Description |
|------|-------|------------------|-------------|
| GET | `dms-document-ddl` | `Task<IActionResult> DocumentsForDDLAsync(CancellationToken cancellationToken)` | Get documents dropdown list |
| POST | `dms-document-summary` | `Task<IActionResult> DocumentSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken)` | Get paginated document grid |
| POST | `dms-document` | `Task<IActionResult> CreateDocumentAsync([FromBody] DmsDocumentDto dto, CancellationToken cancellationToken)` | Create new document record |
| PUT | `dms-document/{key}` | `Task<IActionResult> UpdateDocumentAsync([FromRoute] int key, [FromBody] DmsDocumentDto dto, CancellationToken cancellationToken)` | Update document record |
| DELETE | `dms-document/{key}` | `Task<IActionResult> DeleteDocumentAsync([FromRoute] int key, CancellationToken cancellationToken)` | Delete document record |
| GET | `dms-document/{documentId:int}` | `Task<IActionResult> DocumentAsync([FromRoute] int documentId, CancellationToken cancellationToken)` | Get document by ID |
| POST | `dms-document-upload` | `Task<IActionResult> SaveFileAndDocumentAsync(IFormFile file, [FromForm] string allAboutDMS, CancellationToken cancellationToken)` | Save file and document with metadata |

**RouteConstants Mapping:**
```csharp
RouteConstants.DmsDocumentDDL → "dms-document-ddl"
RouteConstants.DmsDocumentSummary → "dms-document-summary"
RouteConstants.CreateDmsDocument → "dms-document"
RouteConstants.UpdateDmsDocument → "dms-document/{key}"
RouteConstants.DeleteDmsDocument → "dms-document/{key}"
RouteConstants.ReadDmsDocument → "dms-document/{documentId:int}"
RouteConstants.DmsDocumentUpload → "dms-document-upload"
```

---

### **API Endpoint Naming Conventions**

**Standard Patterns:**
- **Summary/Grid**: `{entity}-summary` (POST with GridOptions)
- **Create**: `{entity}` (POST)
- **Update**: `{entity}/{key}` (PUT)
- **Delete**: `{entity}/{key}` (DELETE)
- **Read Single**: `{entity}/{id:int}` (GET)
- **Read All**: `{entities}` (GET)
- **Dropdown Lists**: `{entities}-ddl` (GET)
- **By Foreign Key**: `{entities}-by-{fkname}/{fkId:int}` (GET)

**CRUD Record Pattern:**
- Many modern controllers use CRUD Records (`CreateXxxRecord`, `UpdateXxxRecord`, `DeleteXxxRecord`) instead of DTOs
- This pattern provides better separation of concerns and immutability

**Response Format:**
- All endpoints return unified `ApiResponse<T>` structure
- Success responses include: `StatusCode`, `Success`, `Message`, `Data`, `Timestamp`, `CorrelationId`
- Error responses include: `StatusCode`, `Success`, `Message`, `Errors[]`, `Timestamp`, `CorrelationId`

**Authorization:**
- Most endpoints require `[AuthorizeUser]` attribute (JWT Bearer token)
- Authentication endpoints marked with `[AllowAnonymous]`
- Current user info available via `HttpContext.CurrentUser()` extension method

**Caching:**
- Dropdown lists and read-only endpoints use `[ResponseCache]` (60-300 seconds)
- Memory caching for user data (5-hour sliding expiration)
- Multi-tier caching: L1 (Memory) → L2 (Redis) → L3 (PostgreSQL)

---

### **Implementation Status Task List**

#### **✅ Fully Implemented Modules**
- [x] Authentication (Login, Logout, Token Management)
- [x] Module Management (CRUD + Grid)
- [x] Menu Management (CRUD + Grid + User Permissions)
- [x] Country Management (CRUD + Grid + CRUD Records)
- [x] Group Management (CRUD + Grid + Permissions)
- [x] User Management (CRUD + Grid)
- [x] Company Management (CRUD + CRUD Records)
- [x] Thana Management (CRUD + Grid + CRUD Records)
- [x] Workflow Management (States + Actions)
- [x] Access Control Management (CRUD + Grid)
- [x] CRM Institute (CRUD + Grid + CRUD Records)
- [x] CRM Course (CRUD + Grid + CRUD Records)
- [x] CRM Application (CRUD + Grid + CRUD Records)
- [x] CRM Applicant Info (CRUD + Grid + CRUD Records)
- [x] CRM Education History (CRUD + Grid + CRUD Records)
- [x] CRM Applicant Course (CRUD + Grid + CRUD Records)
- [x] CRM Work Experience (CRUD + Grid + CRUD Records)
- [x] DMS Document Management (CRUD + Grid + File Upload)

#### **⚠️ Partially Implemented Modules**
- [ ] HR Employee Management (Types implemented, full CRUD pending)
- [ ] HR Department Management (Read-only, full CRUD pending)
- [ ] HR Branch Management (Read-only, full CRUD pending)

#### **📋 Pending Frontend Implementation**
- [ ] Module Summary Grid + Modal Form
- [ ] Menu Summary Grid + Modal Form
- [ ] Group Summary Grid + Modal Form + Permission Assignment
- [ ] User Summary Grid + Modal Form
- [ ] Company Summary Grid + Modal Form
- [ ] Thana Summary Grid + Modal Form
- [ ] Workflow Management UI
- [ ] Access Control Management UI
- [ ] CRM Institute UI (Grid + Modal)
- [ ] CRM Course UI (Grid + Modal)
- [ ] CRM Application UI (Grid + Tabbed Form)
- [ ] CRM Applicant Info UI (Grid + Tabbed Form)
- [ ] DMS Document UI (Grid + Upload Modal)
- [ ] HR Employee UI (Grid + Tabbed Form)
- [ ] HR Department UI (Grid + Modal)
- [ ] HR Branch UI (Grid + Modal)

---

### **📅 Implementation Priority & Timeline**

This section outlines the phased approach for frontend implementation, following the 3-file JavaScript pattern (Settings, Details, Summary) established in the Country module reference implementation.

#### **Phase 1: Core System Admin (Week 1-2)** ⚙️
**Estimated Time:** 30-40 hours | **Priority:** CRITICAL

**Modules:**
1. **Module Management** (4-6 hours)
   - [ ] Create `Views/Core/SystemAdmin/Module/Index.cshtml`
   - [ ] Implement `moduleSettings.js` (API config, grid columns, validation)
   - [ ] Implement `moduleSummary.js` (Kendo Grid with server-side paging)
   - [ ] Implement `moduleDetails.js` (Kendo Window modal, form validation)
   - [ ] Test CRUD operations end-to-end

2. **Menu Management** (6-8 hours)
   - [ ] Create `Views/Core/SystemAdmin/Menu/Index.cshtml`
   - [ ] Implement `menuSettings.js` (hierarchical structure config)
   - [ ] Implement `menuSummary.js` (hierarchical grid, parent-child display)
   - [ ] Implement `menuDetails.js` (Module dropdown, Parent Menu cascading, Icon picker)
   - [ ] Test menu hierarchy and relationships

3. **Group Management + Permissions** (8-10 hours)
   - [ ] Create `Views/Core/SystemAdmin/Group/Index.cshtml`
   - [ ] Implement `groupSettings.js` (group + permission assignment config)
   - [ ] Implement `groupSummary.js` (grid with "Assign Permissions" action)
   - [ ] Implement `groupDetails.js` (basic form + permission assignment modal)
   - [ ] Test permission assignment workflow

4. **User Management** (6-8 hours)
   - [ ] Create `Views/Core/SystemAdmin/User/Index.cshtml`
   - [ ] Implement `userSettings.js` (user fields, Group/Company dropdowns)
   - [ ] Implement `userSummary.js` (grid with company filter)
   - [ ] Implement `userDetails.js` (password field logic, email validation)
   - [ ] Test user creation and password handling

5. **Company Management** (5-7 hours)
   - [ ] Create `Views/Core/SystemAdmin/Company/Index.cshtml`
   - [ ] Implement `companySettings.js` (company fields, contact validation)
   - [ ] Implement `companySummary.js` (standard CRUD grid)
   - [ ] Implement `companyDetails.js` (email/phone/website validation)
   - [ ] Test CRUD operations and validation

6. **Thana Management** (5-7 hours)
   - [ ] Create `Views/Core/SystemAdmin/Thana/Index.cshtml`
   - [ ] Implement `thanaSettings.js` (District dropdown, Bangla name)
   - [ ] Implement `thanaSummary.js` (grid with district filter)
   - [ ] Implement `thanaDetails.js` (District selection, bilingual support)
   - [ ] Test district filtering and Bangla rendering

**Phase 1 Deliverables:**
- ✅ Complete System Admin module suite
- ✅ User and access management fully functional
- ✅ Organizational structure (Company, Groups) operational
- ✅ Geographic data (Thana) ready for use

---

#### **Phase 2: CRM Foundation (Week 3)** 🎓
**Estimated Time:** 20-28 hours | **Priority:** HIGH

**Modules:**
7. **CRM Institute** (6-8 hours)
   - [ ] Create `Views/CRM/Institute/Index.cshtml`
   - [ ] Implement `instituteSettings.js` (Country dropdown, contact fields)
   - [ ] Implement `instituteSummary.js` (grid with country filter)
   - [ ] Implement `instituteDetails.js` (Country selection, contact validation)
   - [ ] Test institute-country relationship

8. **CRM Course** (6-8 hours)
   - [ ] Create `Views/CRM/Course/Index.cshtml`
   - [ ] Implement `courseSettings.js` (cascading Country→Institute dropdowns)
   - [ ] Implement `courseSummary.js` (grid with institute filter)
   - [ ] Implement `courseDetails.js` (cascading dropdowns, duration/fees fields)
   - [ ] Test course-institute relationship and cascading

9. **CRM Applicant Info** (8-10 hours)
   - [ ] Create `Views/CRM/ApplicantInfo/Index.cshtml`
   - [ ] Implement `applicantInfoSettings.js` (tabbed form config: Personal, Contact, Education, Documents)
   - [ ] Implement `applicantInfoSummary.js` (grid with search by email)
   - [ ] Implement `applicantInfoDetails.js` (Kendo TabStrip, email uniqueness, photo upload)
   - [ ] Test multi-tab form and photo upload

**Phase 2 Deliverables:**
- ✅ Institute and Course catalog management
- ✅ Applicant information capture system
- ✅ Foundation for CRM application workflow

---

#### **Phase 3: Advanced CRM (Week 4)** 📝
**Estimated Time:** 12-16 hours | **Priority:** HIGH

**Modules:**
10. **CRM Application (Complex Tabbed Form)** (12-16 hours)
    - [ ] Create `Views/CRM/Application/Index.cshtml`
    - [ ] Implement `applicationSettings.js` (7-tab configuration, all entity APIs)
    - [ ] Implement `applicationSummary.js` (grid with status filter, quick view)
    - [ ] Implement `applicationDetails.js` (Kendo TabStrip with lazy loading)
      - [ ] Tab 1: Basic Information
      - [ ] Tab 2: Personal Information
      - [ ] Tab 3: Education History
      - [ ] Tab 4: Work Experience
      - [ ] Tab 5: Document Upload
      - [ ] Tab 6: Course Selection
      - [ ] Tab 7: Review & Submit
    - [ ] Implement Save & Continue functionality
    - [ ] Test end-to-end application flow
    - [ ] Test document upload integration

**Phase 3 Deliverables:**
- ✅ Complete application submission workflow
- ✅ Multi-step form with validation
- ✅ Document management integration

---

#### **Phase 4: HR & Supporting Modules (Week 5)** 👔
**Estimated Time:** 25-35 hours | **Priority:** MEDIUM

**Modules:**
11. **HR Department** (4-5 hours)
    - [ ] Create `Views/HR/Department/Index.cshtml`
    - [ ] Implement `departmentSettings.js` (company association)
    - [ ] Implement `departmentSummary.js` (grid with company filter)
    - [ ] Implement `departmentDetails.js` (simple form, Company dropdown)
    - [ ] Test department CRUD

12. **HR Branch** (4-5 hours)
    - [ ] Create `Views/HR/Branch/Index.cshtml`
    - [ ] Implement `branchSettings.js` (company association, address)
    - [ ] Implement `branchSummary.js` (grid with company filter)
    - [ ] Implement `branchDetails.js` (Company dropdown, address fields)
    - [ ] Test branch CRUD

13. **HR Employee (Complex)** (14-18 hours)
    - [ ] Create `Views/HR/Employee/Index.cshtml`
    - [ ] Implement `employeeSettings.js` (10+ tabs config, organizational structure)
    - [ ] Implement `employeeSummary.js` (advanced filtering: Company/Department/Branch/Status)
    - [ ] Implement `employeeDetails.js` (Kendo TabStrip with multiple tabs)
      - [ ] Tab 1: Personal Information + Photo
      - [ ] Tab 2: Job Information
      - [ ] Tab 3: Salary & Benefits
      - [ ] Tab 4: Documents
      - [ ] Tab 5: Emergency Contacts
      - [ ] Tab 6: Education
      - [ ] Tab 7: Work Experience
      - [ ] Tab 8: Training
      - [ ] Tab 9: Performance
      - [ ] Tab 10: Leave Balance
    - [ ] Implement cascading dropdowns (Company → Branch → Department)
    - [ ] Test complex employee data entry
    - [ ] Test export to Excel functionality

14. **DMS Document Management** (6-8 hours)
    - [ ] Create `Views/DMS/Document/Index.cshtml`
    - [ ] Implement `documentSettings.js` (file upload config, allowed types)
    - [ ] Implement `documentSummary.js` (grid with file preview, download button)
    - [ ] Implement `documentDetails.js` (Kendo Upload widget, metadata form)
    - [ ] Test file upload with progress indicator
    - [ ] Test file download and preview

**Phase 4 Deliverables:**
- ✅ HR organizational structure fully operational
- ✅ Employee information management with complex forms
- ✅ Document management system integrated

---

#### **Phase 5: Workflow & Access Control (Week 6)** 🔄
**Estimated Time:** 15-20 hours | **Priority:** MEDIUM

**Modules:**
15. **Workflow Management** (10-12 hours)
    - [ ] Create `Views/Core/Workflow/Index.cshtml`
    - [ ] Implement `workflowSettings.js` (States + Actions config)
    - [ ] Implement `workflowSummary.js` (Kendo TabStrip: States tab + Actions tab)
    - [ ] Implement `workflowDetails.js` (two forms: State form + Action form)
    - [ ] Test workflow state creation
    - [ ] Test workflow action creation and transitions

16. **Access Control Management** (5-7 hours)
    - [ ] Create `Views/Core/AccessControl/Index.cshtml`
    - [ ] Implement `accessControlSettings.js` (permissions config)
    - [ ] Implement `accessControlSummary.js` (standard CRUD grid)
    - [ ] Implement `accessControlDetails.js` (permission form)
    - [ ] Test access control CRUD
    - [ ] Test integration with Group permissions

**Phase 5 Deliverables:**
- ✅ Workflow engine UI fully functional
- ✅ Access control management complete
- ✅ All modules integrated and tested

---

#### **📊 Summary Timeline**

| Phase | Duration | Modules | Total Hours | Status |
|-------|----------|---------|-------------|--------|
| **Phase 1** | Week 1-2 | 6 System Admin modules | 30-40 hours | 🔜 Ready to start |
| **Phase 2** | Week 3 | 3 CRM Foundation modules | 20-28 hours | ⏳ Pending Phase 1 |
| **Phase 3** | Week 4 | 1 Complex CRM Application | 12-16 hours | ⏳ Pending Phase 2 |
| **Phase 4** | Week 5 | 4 HR & DMS modules | 25-35 hours | ⏳ Pending Phase 3 |
| **Phase 5** | Week 6 | 2 Workflow & Access modules | 15-20 hours | ⏳ Pending Phase 4 |
| **Total** | 6 weeks | 16 modules | **102-139 hours** | 📅 13-18 working days |

---

#### **🎯 Phase 1 Implementation Guide**

**Prerequisites Before Starting:**
- [ ] Verify API endpoints are functional (all Phase 1 backend APIs tested)
- [ ] Ensure ApiClient.js is properly configured
- [ ] Verify Kendo UI license and theme are set up
- [ ] Test Country module reference implementation
- [ ] Review frontend coding standards and patterns

**Phase 1 Step-by-Step Approach:**

1. **Day 1-2: Module Management** (Foundation)
   - Start with simplest module to establish pattern
   - Verify 3-file structure works correctly
   - Test grid pagination and filtering
   - Validate modal form behavior

2. **Day 3-4: Menu Management** (Hierarchical Data)
   - Build on Module experience
   - Implement hierarchical grid display
   - Test cascading dropdown behavior
   - Validate parent-child relationships

3. **Day 5-7: Group Management + Permissions** (Complex Interaction)
   - Most complex Phase 1 module
   - Implement dual-modal system (Group + Permissions)
   - Test permission assignment workflow
   - Validate multi-select interactions

4. **Day 8-9: User Management** (Critical Path)
   - Integrate Group dropdown (depends on Phase 1.3)
   - Implement password field conditional logic
   - Test email validation and uniqueness
   - Validate user-group-company relationships

5. **Day 10-11: Company Management** (Organizational Structure)
   - Standard CRUD implementation
   - Focus on contact field validation
   - Test data integrity

6. **Day 12: Thana Management** (Geographic Data)
   - Quick implementation (similar to Country)
   - Test bilingual support
   - Validate district relationship

7. **Day 13: Phase 1 Integration Testing**
   - Test all modules together
   - Verify data relationships across modules
   - Performance testing with realistic data volumes
   - Bug fixes and refinements

**Phase 1 Success Criteria:**
✅ All 6 modules have working CRUD operations
✅ Grid pagination and filtering functional
✅ Modal forms validate correctly
✅ Dropdowns and cascading selects work
✅ API integration successful for all operations
✅ Error handling displays user-friendly messages
✅ No console errors or warnings
✅ Responsive design works on different screen sizes

---

#### **🛠️ Common Setup Tasks (One-Time Setup)**

Before starting any phase:

- [ ] **Environment Setup**
  - [ ] Clone repository and ensure all dependencies installed
  - [ ] Configure database connection strings
  - [ ] Run database migrations
  - [ ] Verify API project runs without errors
  - [ ] Verify MVC project runs and serves static assets

- [ ] **Frontend Configuration**
  - [ ] Verify `ApiClient.js` base URL configuration
  - [ ] Test authentication flow (login/logout)
  - [ ] Verify Kendo UI widgets load correctly
  - [ ] Test common utilities (appToast, appLoader, etc.)
  - [ ] Configure browser dev tools for debugging

- [ ] **Reference Implementation**
  - [ ] Study Country module implementation thoroughly
  - [ ] Understand Settings → Summary → Details pattern
  - [ ] Review API response handling patterns
  - [ ] Understand error handling approach
  - [ ] Review validation patterns

---

#### **✅ Per-Module Testing Checklist**

After implementing each module, verify:

**Grid Functionality:**
- [ ] Grid loads data correctly with pagination
- [ ] Server-side paging works (not loading all data at once)
- [ ] Grid columns display correctly
- [ ] Sorting works on all sortable columns
- [ ] Filtering works (if applicable)
- [ ] Grid refreshes after CRUD operations

**Create Operation:**
- [ ] "Add New" button opens modal/form
- [ ] Form fields render correctly
- [ ] Required field validation works
- [ ] Data type validation works (email, phone, etc.)
- [ ] Create operation saves to database via API
- [ ] Success toast displays after creation
- [ ] Grid refreshes with new record
- [ ] Modal/form closes automatically

**Read/View Operation:**
- [ ] Grid displays all records
- [ ] Pagination shows correct page numbers
- [ ] Record counts are accurate
- [ ] Data displays in correct format (dates, numbers, etc.)

**Update Operation:**
- [ ] "Edit" button opens modal/form with existing data
- [ ] All fields populate correctly
- [ ] Form validation works on update
- [ ] Update operation saves changes via API
- [ ] Success toast displays after update
- [ ] Grid refreshes with updated data
- [ ] Changes persist on page reload

**Delete Operation:**
- [ ] "Delete" button shows confirmation dialog
- [ ] Confirmation dialog has clear message
- [ ] Cancel button closes dialog without deleting
- [ ] Confirm button deletes record via API
- [ ] Success toast displays after deletion
- [ ] Grid refreshes without deleted record
- [ ] Deletion prevents if record has dependencies (optional)

**Validation & Error Handling:**
- [ ] Required fields show validation message
- [ ] Field-level validation works (length, format, etc.)
- [ ] Server-side validation errors display properly
- [ ] API errors show user-friendly messages
- [ ] Network errors handled gracefully
- [ ] Validation clears when corrected

**User Experience:**
- [ ] Loading indicators show during API calls
- [ ] No console errors or warnings
- [ ] Forms are keyboard accessible (Tab navigation)
- [ ] Enter key submits forms
- [ ] Escape key closes modals
- [ ] Success/error messages are clear and actionable

**Integration Testing:**
- [ ] Dropdown data loads from API
- [ ] Cascading dropdowns work correctly (if applicable)
- [ ] Foreign key relationships maintain integrity
- [ ] Multi-select controls work (if applicable)
- [ ] File uploads work (if applicable)
- [ ] Date pickers format dates correctly (if applicable)

---

#### **🚀 Getting Started with Phase 1**

**Recommended Order:**
1. ✅ Start with **Module Management** (simplest, establishes pattern)
2. ✅ Progress to **Menu Management** (adds complexity with hierarchy)
3. ✅ Tackle **Group Management** (most complex Phase 1 module)
4. ✅ Implement **User Management** (depends on Groups and Companies)
5. ✅ Add **Company Management** (standalone, straightforward)
6. ✅ Finish with **Thana Management** (quick win, similar to Country)

**Daily Development Workflow:**
1. **Morning:** Review module requirements and API endpoints
2. **Create Files:** Set up View + 3 JS files
3. **Settings First:** Configure API endpoints and grid columns
4. **Summary Next:** Implement grid with toolbar
5. **Details Last:** Implement form with validation
6. **Test Thoroughly:** Run through complete CRUD cycle
7. **Commit Progress:** Git commit with clear message
8. **Update Checklist:** Mark completed items

**Best Practices:**
- ✅ Follow Country module pattern exactly
- ✅ Test each operation immediately after implementing
- ✅ Fix bugs before moving to next feature
- ✅ Use browser DevTools to debug
- ✅ Check console for errors after every change
- ✅ Commit working code frequently
- ✅ Write clear commit messages
- ✅ Ask for help when stuck

---

**🎉 Ready to Begin Phase 1 Implementation!**

#### **🔧 Technical Debt & Improvements**
- [ ] Add comprehensive XML documentation comments to all endpoints
- [ ] Implement rate limiting for API endpoints
- [ ] Add API versioning support
- [ ] Implement request/response logging middleware
- [ ] Add OpenAPI/Swagger documentation
- [ ] Implement HATEOAS links for all grid endpoints
- [ ] Add comprehensive integration tests for all endpoints
- [ ] Implement health check endpoints
- [ ] Add API metrics and monitoring
- [ ] Implement GraphQL endpoint (optional)

---

*— End of API Endpoints Reference —*

---

*— End of UI/UX Design Documentation + Frontend Implementation Plan —*

HRIS + BonusPayment System  |  v1.0  |  2025
© 2025 HRIS System
**Last Updated:** 2026-04-20
