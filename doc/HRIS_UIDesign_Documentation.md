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



*— End of UI/UX Design Documentation —*

HRIS + BonusPayment System  |  v1.0  |  2025
© 2025 HRIS System   |   Page 
