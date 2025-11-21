# Frontend Project Structure Analysis
## Equipment Rental System - Vue 3 Frontend

---

## 1. PROJECT OVERVIEW

**Technology Stack:**
- Vue.js 3 (3.5.22)
- Vite (7.1.7) - Build tool
- Vue Router (4.5.1) - Routing
- Pinia (3.0.3) - State management
- Vuetify (3.10.5) - UI component library
- Axios (1.12.2) - HTTP client
- Sass (1.93.2) - CSS preprocessing
- SignalR (@microsoft/signalr 10.0.0) - Real-time communication

**Project Size:**
- Total: 198 MB (includes node_modules)
- Source code only: 236 KB (51 files)
- 10,751 total files (including dependencies)

**Node Version Requirements:** ^20.19.0 || >=22.12.0

---

## 2. DIRECTORY STRUCTURE

```
Midterm_EquipmentRental_Team5_FrontEnd/
├── src/                          # Application source code
│   ├── api/                       # API controllers (backend communication)
│   │   ├── AuthController.js
│   │   ├── CustomerController.js
│   │   ├── EquipmentController.js
│   │   └── RentalController.js
│   ├── components/                # Reusable Vue components
│   │   ├── dialog/                # Dialog/Modal components
│   │   │   ├── AddCustomerDialog.vue
│   │   │   └── equipment/
│   │   │       ├── AddEquipmentDialog.vue
│   │   │       ├── ViewAvailableEquipments.vue
│   │   │       └── ViewRentedEquipments.vue
│   │   ├── forms/                 # Form components
│   │   │   ├── CancelRentalForm.vue
│   │   │   ├── CustomerForm.vue
│   │   │   ├── CustomerIssueRentalForm.vue
│   │   │   ├── CustomerReturnRentalForm.vue
│   │   │   ├── EquipmentForm.vue
│   │   │   ├── ExtendRentalForm.vue
│   │   │   ├── IssueRentalForm.vue
│   │   │   └── ReturnRentalForm.vue
│   │   ├── navigations/           # Navigation components
│   │   │   └── NavigationDrawer.vue
│   │   ├── tables/                # Data table components
│   │   │   ├── customer/
│   │   │   │   ├── CustomerActiveRental.vue
│   │   │   │   ├── CustomerRentedHistory.vue
│   │   │   │   └── CustomerTable.vue
│   │   │   ├── equipment/
│   │   │   │   ├── AvailableEquipmentTable.vue
│   │   │   │   ├── EquipmentTable.vue
│   │   │   │   └── RentedEquipmentTable.vue
│   │   │   └── rentals/
│   │   │       ├── ActiveRentalHistoryTable.vue
│   │   │       ├── CompletedRentalTable.vue
│   │   │       ├── EquipmentRentalHistoryTable.vue
│   │   │       ├── OverdueRentalTable.vue
│   │   │       └── RentalTable.vue
│   │   └── UserActiveRental.vue   # User active rental display component
│   ├── plugins/                   # Vue plugins
│   │   ├── signalr.chat.js        # SignalR chat plugin
│   │   ├── signalr.room.js        # SignalR room plugin
│   │   └── vuetify.js             # Vuetify configuration
│   ├── router/                    # Vue Router configuration
│   │   └── index.js               # Route definitions
│   ├── services/                  # Utility services
│   │   └── RequestHandler.js      # HTTP request utilities
│   ├── stores/                    # Pinia state management
│   │   └── Authentication.js      # Auth store
│   ├── views/                     # Page components
│   │   ├── LoginView.vue          # Login page
│   │   └── dashboard/             # Dashboard section
│   │       ├── DashboardView.vue  # Main dashboard layout
│   │       ├── HomeView.vue       # Dashboard home
│   │       ├── customer/
│   │       │   ├── CustomerDetailsView.vue
│   │       │   └── CustomerView.vue
│   │       ├── equipment/
│   │       │   ├── EquipmentDetailsView.vue
│   │       │   ├── EquipmentRentalHistoryView.vue
│   │       │   └── EquipmentView.vue
│   │       └── rental/
│   │           ├── CancelRentalView.vue
│   │           ├── ExtendRentalView.vue
│   │           ├── IssueEquipmentView.vue
│   │           ├── RentalDetailsView.vue
│   │           ├── RentalView.vue
│   │           └── ReturnEquipmentView.vue
│   ├── App.vue                    # Root component
│   └── main.js                    # Entry point
├── plugins/                       # Root-level plugins directory
│   └── vuetify.js                 # Root Vuetify config (duplicate)
├── public/                        # Static assets
│   └── favicon.ico
├── .vscode/                       # VSCode workspace settings
│   ├── extensions.json            # Recommended extensions
│   ├── extensions 2.json          # DUPLICATE FILE
│   └── settings.json              # VSCode settings
├── node_modules/                  # Dependencies (198 MB)
├── .vite/                         # Vite cache/build files
├── Configuration Files:
│   ├── package.json               # Project dependencies and scripts
│   ├── package-lock.json          # Locked dependency versions
│   ├── vite.config.js             # Vite build configuration
│   ├── eslint.config.js           # ESLint rules
│   ├── jsconfig.json              # JS path aliases
│   ├── .prettierrc.json           # Code formatting rules
│   ├── .editorconfig              # Editor consistency rules
│   ├── .gitignore                 # Git ignore patterns
│   ├── .gitattributes             # Git attribute settings
│   └── .DS_Store                  # macOS file metadata (should be ignored)
└── index.html                     # HTML entry point
```

---

## 3. KEY FILES IN SRC/ FOLDER

| File | Purpose | Dependencies |
|------|---------|---|
| **main.js** | Application entry point | Vue, Pinia, Vuetify, Router |
| **App.vue** | Root Vue component | RouterView, SignalR |
| **router/index.js** | Route definitions & guards | Vue Router, Authentication Store |
| **stores/Authentication.js** | Auth state management | Pinia, Persistence plugin |
| **services/RequestHandler.js** | HTTP request utilities | Axios |
| **api/AuthController.js** | Auth API calls | Axios, RequestHandler |
| **api/CustomerController.js** | Customer CRUD API | Axios, RequestHandler |
| **api/EquipmentController.js** | Equipment CRUD API | Axios, RequestHandler |
| **api/RentalController.js** | Rental operation API | Axios, RequestHandler |
| **plugins/vuetify.js** | Vuetify theme & config | Vuetify |
| **plugins/signalr.*.js** | Real-time comm setup | @microsoft/signalr |

---

## 4. CONFIGURATION FILES ANALYSIS

### package.json
```json
{
  "name": "vue-project",
  "version": "0.0.0",
  "type": "module",
  "scripts": {
    "dev": "vite",
    "build": "vite build",
    "preview": "vite preview",
    "lint": "eslint . --fix",
    "format": "prettier --write src/"
  }
}
```

**Scripts Available:**
- `npm run dev` - Start dev server
- `npm run build` - Production build
- `npm run preview` - Preview prod build
- `npm run lint` - Fix ESLint issues
- `npm run format` - Format code with Prettier

### vite.config.js
- **Plugins:** Vue support, Vue DevTools
- **Path Alias:** @ -> ./src
- **Build Tool:** Vite with optimizations

### eslint.config.js
- **Rules:** Vue essentials + Prettier formatting
- **Scope:** .js, .mjs, .jsx, .vue files
- **Ignored:** dist/, coverage/

### .prettierrc.json
- Semi-colons: Off
- Quotes: Single quotes
- Line width: 100 characters

### .editorconfig
- Indent: 2 spaces
- Charset: UTF-8
- Line ending: LF
- Final newline: Required

### jsconfig.json
- **Path alias:** @/* -> ./src/*
- **Excluded:** node_modules/, dist/

---

## 5. COMPONENT ORGANIZATION

### By Type:
**API Controllers (4 files)**
- Single responsibility: Backend communication
- Located: `/src/api/`

**Views (13 files)**
- Pages/Screen components
- Located: `/src/views/`
- Lazy-loaded in router

**Components (23 files)**
- Organized by purpose (forms, tables, dialogs, navigation)
- Located: `/src/components/`
- Reusable across views

**Plugins (3 files)**
- Vuetify config (1 file + 1 duplicate)
- SignalR real-time (2 files)
- Located: `/src/plugins/` and `/plugins/`

---

## 6. UNUSED AND REDUNDANT FILES

### Potential Issues:

1. **DUPLICATE FILE** - VSCode Extensions Config
   - `/vscode/extensions 2.json` (backup/duplicate)
   - Recommendation: DELETE
   - Impact: None (VSCode ignores duplicates)

2. **DUPLICATE PLUGIN CONFIG**
   - `/plugins/vuetify.js` (root level)
   - `/src/plugins/vuetify.js` (actual used location)
   - Recommendation: DELETE `/plugins/vuetify.js`
   - Status: Root version appears unused

3. **UNUSED COMPONENT** - UserActiveRental.vue
   - Location: `/src/components/UserActiveRental.vue`
   - Usage: Imported in 2 places:
     - `CustomerReturnRentalForm.vue`
     - `RentalView.vue`
   - Status: NOT UNUSED - actually used
   - Finding: False alarm - component is referenced

4. **POTENTIAL MISSING SERVICE**
   - `App.vue` imports from './services/signalr'
   - Missing file: `/src/services/signalr.js`
   - Only found: `/src/plugins/signalr.*.js`
   - Issue: Mismatch in import path
   - Recommendation: Create `/src/services/signalr.js` or update import

5. **UNUSED SIGNALR PLUGINS** (Possible)
   - `/src/plugins/signalr.chat.js`
   - `/src/plugins/signalr.room.js`
   - Status: Not explicitly imported in main.js
   - Recommendation: Verify if these are used, else delete

---

## 7. OVERALL PROJECT ORGANIZATION

### Strengths:
✓ Clear separation of concerns
✓ Feature-based folder structure (api, components, views, stores, router)
✓ Consistent naming conventions
✓ Lazy-loaded routes for performance
✓ Proper configuration files for linting/formatting
✓ VSCode workspace configured with recommended extensions
✓ State management with Pinia + persistence
✓ Type hints via JSConfig path aliases

### Areas for Improvement:
⚠ Duplicate files need cleanup (.vscode/extensions 2.json)
⚠ Duplicate plugin config (/plugins/ vs /src/plugins/)
⚠ Missing signalr.js service import issue in App.vue
⚠ Project title in index.html is generic ("Vite App")
⚠ Unused SignalR plugin files (not imported)
⚠ .DS_Store in repo (should be in .gitignore)
⚠ Some form components are very similar (customer/admin variants)

### Scalability:
- Good architecture for mid-size project
- Component structure supports adding new features
- API controllers pattern is scalable
- Could benefit from: shared utilities folder, hooks folder, constants folder

---

## 8. DEPENDENCY SUMMARY

### Production Dependencies (7):
- vue (3.5.22) - Framework
- vue-router (4.5.1) - Routing
- pinia (3.0.3) - State management
- pinia-plugin-persistedstate (4.5.0) - Persist state
- vuetify (3.10.5) - UI components
- @mdi/font (7.4.47) - Material icons
- @microsoft/signalr (10.0.0) - Real-time communication
- axios (1.12.2) - HTTP client
- sass (1.93.2) - CSS preprocessing

### Dev Dependencies (8):
- vite (7.1.7) - Build tool
- @vitejs/plugin-vue (6.0.1) - Vue support for Vite
- eslint (9.33.0) - Code linting
- eslint-plugin-vue (~10.4.0) - Vue linting
- @vue/eslint-config-prettier (10.2.0) - Prettier integration
- prettier (3.6.2) - Code formatting
- vite-plugin-vue-devtools (8.0.2) - Vue DevTools
- @eslint/js (9.33.0) - JS linting rules
- globals (16.3.0) - Global variables

---

## 9. FILE COUNT BREAKDOWN

| Category | Count |
|----------|-------|
| Vue Components (.vue) | 38 |
| JavaScript Files (.js) | 13 |
| Configuration Files | 8 |
| Static Assets | 1 |
| **Total Source Code** | **51** |
| node_modules | 10,700+ |
| **Grand Total** | **10,751** |

---

## 10. RECOMMENDATIONS

### Immediate Actions:
1. Delete `.vscode/extensions 2.json` (duplicate)
2. Delete `/plugins/vuetify.js` (duplicate - use `/src/plugins/vuetify.js`)
3. Investigate App.vue import path for SignalR service
4. Update index.html title from "Vite App" to "Equipment Rental System"
5. Add .DS_Store to .gitignore (or remove it from repo)

### Medium-term Improvements:
1. Create `/src/utils/` for shared utility functions
2. Create `/src/constants/` for app-wide constants
3. Review and remove unused SignalR plugins if not needed
4. Consolidate similar form components
5. Add a `/src/composables/` folder for Vue composables

### Code Quality:
1. All ESLint and Prettier configs are in place
2. Consider adding unit tests (Vitest)
3. Type safety: Consider using TypeScript or JSDoc
4. Add tests for authentication guards

