# Lecture 2: Modern CSS — SCSS, Flexbox & Tailwind CSS

**Duration:** 4 hours (with breaks)
**Prerequisites:** Basic HTML & CSS knowledge, ASP.NET MVC project from Lecture 1
**Goal:** Master three pillars of modern CSS development — preprocessors (SCSS), layout (Flexbox), and utility-first frameworks (Tailwind CSS)

---

## Table of Contents

1. [Part 1: SCSS / Sass (75 min)](#part-1-scss--sass-75-min)
   - 1.1 What Is a CSS Preprocessor?
   - 1.2 SCSS vs Sass Syntax
   - 1.3 Variables
   - 1.4 Nesting
   - 1.5 Partials and Imports
   - 1.6 Mixins
   - 1.7 Extend / Inheritance
   - 1.8 Functions and Operators
   - 1.9 Control Directives (@if, @for, @each)
   - 1.10 Practical Exercise: Theme System
2. [Part 2: CSS Flexbox (75 min)](#part-2-css-flexbox-75-min)
   - 2.1 The Box Model Recap
   - 2.2 Display Types: block, inline, inline-block, flex
   - 2.3 Flex Container Properties
   - 2.4 Flex Item Properties
   - 2.5 Common Layout Patterns
   - 2.6 Responsive Design with Flexbox
   - 2.7 Flexbox vs Grid (When to Use Which)
   - 2.8 Practical Exercise: Navigation + Card Layout
3. [Part 3: Tailwind CSS (75 min)](#part-3-tailwind-css-75-min)
   - 3.1 What Is Utility-First CSS?
   - 3.2 Tailwind vs Traditional CSS vs Component Frameworks
   - 3.3 Setup and Installation
   - 3.4 Core Utility Classes
   - 3.5 Responsive Design in Tailwind
   - 3.6 State Variants (hover, focus, active)
   - 3.7 Dark Mode
   - 3.8 Customizing Tailwind (tailwind.config.js)
   - 3.9 Extracting Components (@apply)
   - 3.10 Practical Exercise: Convert Materialize to Tailwind
4. [Part 4: Putting It All Together (15 min)](#part-4-putting-it-all-together-15-min)
   - 4.1 SCSS + Tailwind: Can They Coexist?
   - 4.2 Choosing the Right Tool
   - 4.3 Summary and Q&A

---

## Part 1: SCSS / Sass (75 min)

### 1.1 What Is a CSS Preprocessor?

A **CSS preprocessor** is a scripting language that extends CSS and compiles into regular CSS. The browser never sees SCSS — it only sees the compiled `.css` output.

**Why do we need preprocessors?**

Plain CSS has real limitations that become painful in large projects:
- No variables (CSS custom properties came later, but with limitations)
- No nesting — leads to repetitive, flat selectors
- No code reuse mechanisms (mixins, functions)
- No math operations (before `calc()`)
- No modularity system

**Popular CSS Preprocessors:**

| Preprocessor | File Extension | Syntax | Popularity |
|-------------|---------------|--------|-----------|
| **Sass/SCSS** | `.scss` / `.sass` | CSS-like / Indented | Most popular |
| Less | `.less` | CSS-like | Declining |
| Stylus | `.styl` | Flexible | Niche |

**How compilation works:**

```
SCSS Source (.scss) → Sass Compiler → CSS Output (.css) → Browser
```

The compiler can be:
- **CLI tool**: `sass input.scss output.css`
- **Build tool plugin**: Webpack, Vite, Gulp
- **IDE extension**: Live Sass Compiler in VS Code
- **Framework integration**: ASP.NET has built-in SCSS support via `WebCompiler`

### 1.2 SCSS vs Sass Syntax

There are two syntaxes for the Sass preprocessor:

**SCSS (Sassy CSS)** — uses braces and semicolons, superset of CSS:

```scss
$primary: #3498db;

.navbar {
  background-color: $primary;
  
  .nav-link {
    color: white;
    
    &:hover {
      color: darken(white, 10%);
    }
  }
}
```

**Sass (Indented Syntax)** — uses indentation, no braces/semicolons:

```sass
$primary: #3498db

.navbar
  background-color: $primary
  
  .nav-link
    color: white
    
    &:hover
      color: darken(white, 10%)
```

**We use SCSS** because:
- It's a **superset of CSS** — any valid CSS is valid SCSS
- More familiar to developers coming from CSS
- Better tooling support
- Industry standard

### 1.3 Variables

Variables store values you want to reuse throughout your stylesheet.

**Syntax:** `$variable-name: value;`

```scss
// Color palette
$primary-color: #3498db;
$secondary-color: #2ecc71;
$danger-color: #e74c3c;
$warning-color: #f39c12;

// Typography
$font-family-base: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
$font-size-base: 16px;
$font-size-lg: 1.25rem;
$font-size-sm: 0.875rem;
$line-height-base: 1.6;

// Spacing
$spacing-unit: 8px;
$spacing-sm: $spacing-unit * 1;   // 8px
$spacing-md: $spacing-unit * 2;   // 16px
$spacing-lg: $spacing-unit * 3;   // 24px
$spacing-xl: $spacing-unit * 4;   // 32px

// Breakpoints
$breakpoint-sm: 576px;
$breakpoint-md: 768px;
$breakpoint-lg: 992px;
$breakpoint-xl: 1200px;

// Usage
body {
  font-family: $font-family-base;
  font-size: $font-size-base;
  line-height: $line-height-base;
}

.btn-primary {
  background-color: $primary-color;
  padding: $spacing-sm $spacing-md;
}
```

**Variable scope:**

```scss
$color: red; // global

.parent {
  $color: blue; // local to .parent — does NOT change global
  color: $color; // blue
}

.sibling {
  color: $color; // red (global unchanged)
}
```

**The `!default` flag:**

```scss
$primary-color: #3498db !default;
```

This means: "Use `#3498db` only if `$primary-color` hasn't been defined yet." Essential for creating configurable libraries.

**SCSS Variables vs CSS Custom Properties:**

| Feature | SCSS Variables | CSS Custom Properties |
|---------|---------------|----------------------|
| Syntax | `$var: value` | `--var: value` |
| Scope | Compile-time | Runtime (cascading) |
| Dynamic | No | Yes (JS can change) |
| Fallback | No | Yes: `var(--x, fallback)` |
| Media queries | Not reactive | Reactive |

### 1.4 Nesting

Nesting lets you write selectors that mirror your HTML structure.

```scss
// Instead of this flat CSS:
.navbar { background: #333; }
.navbar ul { list-style: none; }
.navbar ul li { display: inline-block; }
.navbar ul li a { color: white; text-decoration: none; }
.navbar ul li a:hover { color: #3498db; }

// Write this nested SCSS:
.navbar {
  background: #333;

  ul {
    list-style: none;

    li {
      display: inline-block;

      a {
        color: white;
        text-decoration: none;

        &:hover {
          color: #3498db;
        }
      }
    }
  }
}
```

**The `&` (parent selector):**

The `&` refers to the parent selector. It's essential for pseudo-classes, pseudo-elements, and BEM methodology.

```scss
.button {
  background: blue;
  color: white;
  
  // Pseudo-classes
  &:hover { background: darkblue; }
  &:active { transform: scale(0.98); }
  &:focus { outline: 2px solid yellow; }
  
  // Pseudo-elements
  &::before { content: '→ '; }
  &::after { content: ' ←'; }
  
  // BEM modifiers
  &--primary { background: #3498db; }
  &--danger { background: #e74c3c; }
  &--large { padding: 16px 32px; font-size: 1.2rem; }
  
  // BEM elements
  &__icon { margin-right: 8px; }
  &__text { font-weight: bold; }
  
  // Context-dependent styles
  .dark-theme & { background: #555; }
}
```

Compiles to:
```css
.button { background: blue; color: white; }
.button:hover { background: darkblue; }
.button:active { transform: scale(0.98); }
.button:focus { outline: 2px solid yellow; }
.button::before { content: '→ '; }
.button::after { content: ' ←'; }
.button--primary { background: #3498db; }
.button--danger { background: #e74c3c; }
.button--large { padding: 16px 32px; font-size: 1.2rem; }
.button__icon { margin-right: 8px; }
.button__text { font-weight: bold; }
.dark-theme .button { background: #555; }
```

**Warning — don't nest too deep!**

```scss
// BAD: Over-nesting (produces overly specific selectors)
.page {
  .content {
    .sidebar {
      .widget {
        .widget-title {
          .icon {
            color: red; // Compiles to: .page .content .sidebar .widget .widget-title .icon
          }
        }
      }
    }
  }
}

// GOOD: Keep nesting to 3 levels max
.widget {
  &__title {
    .icon { color: red; }
  }
}
```

**The Inception Rule:** Never nest more than 3 levels deep.

### 1.5 Partials and Imports

Partials let you split your CSS into smaller, maintainable files.

**Naming convention:** Prefix partial files with underscore: `_variables.scss`

**Project structure:**

```
scss/
├── abstracts/
│   ├── _variables.scss    // All variables
│   ├── _mixins.scss       // All mixins
│   └── _functions.scss    // All functions
├── base/
│   ├── _reset.scss        // CSS reset / normalize
│   ├── _typography.scss   // Font definitions
│   └── _base.scss         // Base element styles
├── components/
│   ├── _buttons.scss
│   ├── _cards.scss
│   ├── _navbar.scss
│   └── _forms.scss
├── layout/
│   ├── _header.scss
│   ├── _footer.scss
│   ├── _sidebar.scss
│   └── _grid.scss
├── pages/
│   ├── _home.scss
│   ├── _about.scss
│   └── _contact.scss
└── main.scss              // Main entry file
```

**main.scss** (the entry point that imports everything):

```scss
// Abstracts (no CSS output by themselves)
@use 'abstracts/variables';
@use 'abstracts/mixins';
@use 'abstracts/functions';

// Base styles
@use 'base/reset';
@use 'base/typography';
@use 'base/base';

// Layout
@use 'layout/header';
@use 'layout/footer';
@use 'layout/sidebar';
@use 'layout/grid';

// Components
@use 'components/buttons';
@use 'components/cards';
@use 'components/navbar';
@use 'components/forms';

// Page-specific
@use 'pages/home';
@use 'pages/about';
@use 'pages/contact';
```

**`@use` vs `@import` (Modern Sass):**

`@import` is **deprecated**. Use `@use` and `@forward` instead:

```scss
// _variables.scss
$primary: #3498db;
$secondary: #2ecc71;

// _buttons.scss — using @use
@use 'variables';

.btn-primary {
  background: variables.$primary; // namespaced access
}

// Or with a custom namespace:
@use 'variables' as vars;
.btn-primary {
  background: vars.$primary;
}

// Or with no namespace (use with caution):
@use 'variables' as *;
.btn-primary {
  background: $primary;
}
```

**`@forward`** re-exports another module's members:

```scss
// abstracts/_index.scss
@forward 'variables';
@forward 'mixins';
@forward 'functions';

// components/_buttons.scss
@use '../abstracts' as *; // gets everything forwarded
```

### 1.6 Mixins

Mixins are reusable blocks of CSS declarations. Think of them as functions that output CSS.

**Basic mixin:**

```scss
@mixin flex-center {
  display: flex;
  justify-content: center;
  align-items: center;
}

.hero {
  @include flex-center;
  height: 100vh;
}

.card-header {
  @include flex-center;
  padding: 16px;
}
```

**Mixin with parameters:**

```scss
@mixin button($bg-color, $text-color: white, $radius: 4px) {
  background-color: $bg-color;
  color: $text-color;
  border: none;
  border-radius: $radius;
  padding: 10px 20px;
  cursor: pointer;
  transition: background-color 0.2s ease;

  &:hover {
    background-color: darken($bg-color, 10%);
  }
}

.btn-primary { @include button(#3498db); }
.btn-danger  { @include button(#e74c3c); }
.btn-success { @include button(#2ecc71, white, 20px); }
```

**Responsive breakpoint mixin (very common):**

```scss
@mixin respond-to($breakpoint) {
  @if $breakpoint == 'sm' {
    @media (min-width: 576px) { @content; }
  } @else if $breakpoint == 'md' {
    @media (min-width: 768px) { @content; }
  } @else if $breakpoint == 'lg' {
    @media (min-width: 992px) { @content; }
  } @else if $breakpoint == 'xl' {
    @media (min-width: 1200px) { @content; }
  }
}

.container {
  width: 100%;
  padding: 0 16px;

  @include respond-to('sm') { max-width: 540px; }
  @include respond-to('md') { max-width: 720px; }
  @include respond-to('lg') { max-width: 960px; }
  @include respond-to('xl') { max-width: 1140px; }
}
```

**Advanced: mixin with @content block:**

```scss
@mixin on-hover {
  &:hover,
  &:focus {
    @content;
  }
}

.card {
  background: white;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);

  @include on-hover {
    box-shadow: 0 8px 16px rgba(0,0,0,0.2);
    transform: translateY(-2px);
  }
}
```

**Practical mixins collection:**

```scss
@mixin truncate($lines: 1) {
  @if $lines == 1 {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
  } @else {
    display: -webkit-box;
    -webkit-line-clamp: $lines;
    -webkit-box-orient: vertical;
    overflow: hidden;
  }
}

@mixin aspect-ratio($width, $height) {
  position: relative;
  
  &::before {
    content: '';
    display: block;
    padding-top: ($height / $width) * 100%;
  }
  
  > * {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
  }
}

@mixin grid($columns, $gap: 16px) {
  display: grid;
  grid-template-columns: repeat($columns, 1fr);
  gap: $gap;
}
```

### 1.7 Extend / Inheritance

`@extend` lets one selector inherit styles from another.

```scss
%message-shared {
  border: 1px solid #ccc;
  padding: 10px;
  border-radius: 4px;
  margin-bottom: 16px;
}

.message-success {
  @extend %message-shared;
  border-color: #2ecc71;
  background-color: #d4edda;
  color: #155724;
}

.message-error {
  @extend %message-shared;
  border-color: #e74c3c;
  background-color: #f8d7da;
  color: #721c24;
}

.message-warning {
  @extend %message-shared;
  border-color: #f39c12;
  background-color: #fff3cd;
  color: #856404;
}
```

**`%placeholder` selectors** — these only appear in output when extended, never on their own.

**@extend vs @mixin — when to use which:**

| Feature | @extend | @mixin |
|---------|---------|--------|
| Parameters | No | Yes |
| Output | Grouped selectors | Duplicated declarations |
| File size | Smaller | Larger |
| Media queries | Cannot cross | Can cross |
| Best for | Identical styles | Parameterized styles |

**Rule of thumb:** If you need parameters, use `@mixin`. If styles are identical, use `@extend`.

### 1.8 Functions and Operators

**Arithmetic operators:**

```scss
$base-size: 16px;

.container {
  width: 100% - 20px;       // subtraction (same units only with %)
  font-size: $base-size * 1.5; // multiplication
  padding: $base-size / 2;     // division (deprecated, use math.div)
  margin: $base-size + 4px;    // addition
}
```

**Modern division (Dart Sass):**

```scss
@use 'sass:math';

.element {
  width: math.div(100%, 3);      // 33.3333%
  padding: math.div($spacing, 2); // half spacing
}
```

**Built-in color functions:**

```scss
$base-color: #3498db;

.element {
  background: $base-color;
  border-color: darken($base-color, 15%);      // darker shade
  color: lighten($base-color, 30%);             // lighter shade
  outline: adjust-hue($base-color, 45deg);      // shift hue
  box-shadow: 0 2px 4px rgba($base-color, 0.3); // add alpha
}

// Generating shades:
$color-100: lighten($base-color, 40%);
$color-200: lighten($base-color, 30%);
$color-300: lighten($base-color, 20%);
$color-400: lighten($base-color, 10%);
$color-500: $base-color;
$color-600: darken($base-color, 10%);
$color-700: darken($base-color, 20%);
$color-800: darken($base-color, 30%);
$color-900: darken($base-color, 40%);
```

**Custom functions:**

```scss
@function rem($pixels, $base: 16) {
  @return math.div($pixels, $base) * 1rem;
}

@function em($pixels, $base: 16) {
  @return math.div($pixels, $base) * 1em;
}

@function color-contrast($color) {
  $luminance: red($color) * 0.299 + green($color) * 0.587 + blue($color) * 0.114;
  @return if($luminance > 128, #000, #fff);
}

// Usage
h1 {
  font-size: rem(32);      // 2rem
  margin-bottom: rem(16);  // 1rem
}

.badge {
  $bg: #e74c3c;
  background: $bg;
  color: color-contrast($bg); // automatically white or black
}
```

**String functions:**

```scss
@use 'sass:string';

$font: 'Helvetica Neue';
// string.quote(), string.unquote(), string.length(), string.index(), etc.
```

### 1.9 Control Directives (@if, @for, @each, @while)

**@if / @else:**

```scss
@mixin theme($mode) {
  @if $mode == 'dark' {
    background-color: #1a1a2e;
    color: #eee;
  } @else if $mode == 'light' {
    background-color: #fff;
    color: #333;
  } @else {
    background-color: #f5f5f5;
    color: #666;
  }
}

body { @include theme('dark'); }
```

**@for loop:**

```scss
// Generating spacing utilities (like Tailwind does internally)
@for $i from 0 through 8 {
  .mt-#{$i} { margin-top: $i * 8px; }
  .mb-#{$i} { margin-bottom: $i * 8px; }
  .ml-#{$i} { margin-left: $i * 8px; }
  .mr-#{$i} { margin-right: $i * 8px; }
  .p-#{$i}  { padding: $i * 8px; }
}

// Generates: .mt-0 { margin-top: 0; } .mt-1 { margin-top: 8px; } ... etc.

// Grid columns
@for $i from 1 through 12 {
  .col-#{$i} {
    width: math.div(100%, 12) * $i;
  }
}
```

**@each loop:**

```scss
$social-colors: (
  'facebook': #3b5998,
  'twitter': #1da1f2,
  'instagram': #e1306c,
  'linkedin': #0077b5,
  'youtube': #ff0000,
);

@each $network, $color in $social-colors {
  .btn-#{$network} {
    background-color: $color;
    color: white;
    
    &:hover {
      background-color: darken($color, 10%);
    }
  }
}

// Multiple assignment
$icons: ('home', '🏠'), ('mail', '✉️'), ('phone', '📞');

@each $name, $emoji in $icons {
  .icon-#{$name}::before {
    content: $emoji;
  }
}
```

**@while loop:**

```scss
$columns: 12;
$i: 1;

@while $i <= $columns {
  .col-#{$i} {
    width: math.div(100%, $columns) * $i;
  }
  $i: $i + 1;
}
```

### 1.10 Practical Exercise: Theme System

**Build a complete theme system using everything learned:**

```scss
// _variables.scss
$themes: (
  'light': (
    'bg-primary': #ffffff,
    'bg-secondary': #f8f9fa,
    'text-primary': #212529,
    'text-secondary': #6c757d,
    'accent': #3498db,
    'border': #dee2e6,
  ),
  'dark': (
    'bg-primary': #1a1a2e,
    'bg-secondary': #16213e,
    'text-primary': #eee,
    'text-secondary': #adb5bd,
    'accent': #5dade2,
    'border': #2c3e50,
  ),
);

// _mixins.scss
@mixin themed() {
  @each $theme-name, $theme-map in $themes {
    .theme-#{$theme-name} & {
      $theme-map: $theme-map !global;
      @content;
    }
  }
}

@function t($key) {
  @return map-get($theme-map, $key);
}

// _card.scss
.card {
  border-radius: 8px;
  padding: 24px;
  margin-bottom: 16px;

  @include themed() {
    background: t('bg-primary');
    color: t('text-primary');
    border: 1px solid t('border');
  }
}

.card__title {
  font-size: 1.25rem;
  font-weight: 600;
  margin-bottom: 8px;

  @include themed() {
    color: t('accent');
  }
}
```

---

> **BREAK (10 minutes)**

---

## Part 2: CSS Flexbox (75 min)

### 2.1 The Box Model Recap

Every HTML element is a box with four layers:

```
┌─────────────────────── margin ───────────────────────┐
│ ┌─────────────────── border ───────────────────────┐ │
│ │ ┌───────────────── padding ──────────────────┐   │ │
│ │ │ ┌──────────────── content ──────────────┐  │   │ │
│ │ │ │                                       │  │   │ │
│ │ │ │        width × height                 │  │   │ │
│ │ │ │                                       │  │   │ │
│ │ │ └───────────────────────────────────────┘  │   │ │
│ │ └────────────────────────────────────────────┘   │ │
│ └──────────────────────────────────────────────────┘ │
└──────────────────────────────────────────────────────┘
```

**`box-sizing: border-box`** is essential — it makes `width` include padding and border:

```scss
*, *::before, *::after {
  box-sizing: border-box;
}
```

Without `border-box`:
- `width: 300px` + `padding: 20px` + `border: 1px` = **342px actual width**

With `border-box`:
- `width: 300px` includes everything = **300px actual width**

### 2.2 Display Types

Before Flexbox, we had limited layout tools:

| Display | Behavior |
|---------|----------|
| `block` | Full width, starts new line (div, h1, p) |
| `inline` | Only as wide as content, no width/height (span, a, em) |
| `inline-block` | Inline flow but accepts width/height |
| `none` | Removed from layout |
| **`flex`** | Flex container — our focus |
| `grid` | Grid container |

### 2.3 Flex Container Properties

When you set `display: flex` on an element, it becomes a **flex container** and its direct children become **flex items**.

```html
<div class="container">   <!-- flex container -->
  <div class="item">1</div>  <!-- flex item -->
  <div class="item">2</div>  <!-- flex item -->
  <div class="item">3</div>  <!-- flex item -->
</div>
```

#### flex-direction

Controls the **main axis** direction.

```scss
.container {
  display: flex;
  
  // flex-direction: row;            // ← default: left to right →
  // flex-direction: row-reverse;    // → right to left ←
  // flex-direction: column;         // ↓ top to bottom ↓
  // flex-direction: column-reverse; // ↑ bottom to top ↑
}
```

```
flex-direction: row
┌──────────────────────────────┐
│ [Item 1] [Item 2] [Item 3]  │
│  ───────→ main axis          │
│  │                           │
│  ↓ cross axis                │
└──────────────────────────────┘

flex-direction: column
┌────────────┐
│ [Item 1]   │
│ [Item 2]   │  │ main axis
│ [Item 3]   │  ↓
│  ──→ cross │
└────────────┘
```

#### justify-content

Aligns items along the **main axis**.

```scss
.container {
  display: flex;
  
  // justify-content: flex-start;    // [1][2][3]............
  // justify-content: flex-end;      // ............[1][2][3]
  // justify-content: center;        // ......[1][2][3]......
  // justify-content: space-between; // [1]......[2]......[3]
  // justify-content: space-around;  // ..[1]....[2]....[3]..
  // justify-content: space-evenly;  // ...[1]...[2]...[3]...
}
```

Visual representation:

```
flex-start:      |[A][B][C]                    |
flex-end:        |                    [A][B][C]|
center:          |          [A][B][C]          |
space-between:   |[A]        [B]        [C]   |
space-around:    |  [A]     [B]     [C]       |
space-evenly:    |   [A]    [B]    [C]        |
```

#### align-items

Aligns items along the **cross axis**.

```scss
.container {
  display: flex;
  height: 200px;
  
  // align-items: stretch;     // default — items fill full height
  // align-items: flex-start;  // items at top
  // align-items: flex-end;    // items at bottom
  // align-items: center;      // items centered vertically
  // align-items: baseline;    // items aligned by text baseline
}
```

```
align-items: stretch       align-items: flex-start
┌──────────────────┐       ┌──────────────────┐
│ ┌──┐┌──┐┌──┐    │       │ [A] [B] [C]      │
│ │A ││B ││C │    │       │                   │
│ │  ││  ││  │    │       │                   │
│ └──┘└──┘└──┘    │       └──────────────────┘
└──────────────────┘

align-items: center        align-items: flex-end
┌──────────────────┐       ┌──────────────────┐
│                  │       │                   │
│ [A] [B] [C]     │       │                   │
│                  │       │ [A] [B] [C]       │
└──────────────────┘       └──────────────────┘
```

#### flex-wrap

Controls whether items wrap to the next line.

```scss
.container {
  display: flex;
  
  // flex-wrap: nowrap;       // default — all items on one line (may overflow)
  // flex-wrap: wrap;         // items wrap to next line
  // flex-wrap: wrap-reverse; // items wrap to previous line
}
```

```
nowrap (items might shrink or overflow):
┌──────────────────────────────┐
│ [1][2][3][4][5][6][7][8][9]  │
└──────────────────────────────┘

wrap:
┌──────────────────────────────┐
│ [1] [2] [3] [4] [5]         │
│ [6] [7] [8] [9]             │
└──────────────────────────────┘
```

#### align-content

When items wrap to multiple lines, `align-content` controls spacing between lines (only works with `flex-wrap: wrap`).

```scss
.container {
  display: flex;
  flex-wrap: wrap;
  
  // align-content: flex-start;
  // align-content: flex-end;
  // align-content: center;
  // align-content: space-between;
  // align-content: space-around;
  // align-content: stretch;        // default
}
```

#### gap

Modern spacing between flex items (replaces margin hacks):

```scss
.container {
  display: flex;
  gap: 16px;           // equal row and column gap
  // row-gap: 16px;    // gap between rows
  // column-gap: 24px; // gap between columns
}
```

### 2.4 Flex Item Properties

#### flex-grow

How much an item should grow relative to other items when there's extra space.

```scss
.item-1 { flex-grow: 1; }  // takes 1 part of extra space
.item-2 { flex-grow: 2; }  // takes 2 parts of extra space
.item-3 { flex-grow: 1; }  // takes 1 part of extra space
```

```
Available space: |=========================|
After distribution:
┌─────────┬──────────────────┬─────────┐
│ Item 1  │     Item 2       │ Item 3  │
│ (1/4)   │     (2/4)        │ (1/4)   │
└─────────┴──────────────────┴─────────┘
```

#### flex-shrink

How much an item should shrink when there's not enough space. Default is `1`.

```scss
.item-1 { flex-shrink: 1; } // shrinks normally
.item-2 { flex-shrink: 0; } // refuses to shrink
.item-3 { flex-shrink: 2; } // shrinks twice as much
```

#### flex-basis

The initial size of an item before growing/shrinking. Like `width` but for the main axis.

```scss
.item {
  flex-basis: 200px; // start at 200px, then grow/shrink
  // flex-basis: 25%; // or use percentages
  // flex-basis: auto; // default — use item's content size
}
```

#### The flex shorthand

```scss
.item {
  // flex: grow shrink basis
  flex: 0 1 auto;   // default: don't grow, can shrink, auto basis
  flex: 1;          // shorthand for: 1 1 0% (grow equally, basis 0)
  flex: 1 0 200px;  // grow from 200px, don't shrink
  flex: none;       // shorthand for: 0 0 auto (rigid)
}
```

#### align-self

Overrides `align-items` for individual items.

```scss
.container {
  display: flex;
  align-items: flex-start;
}

.special-item {
  align-self: center; // this item centers itself
}
```

#### order

Changes visual order without changing HTML order.

```scss
.item-1 { order: 3; } // appears third
.item-2 { order: 1; } // appears first
.item-3 { order: 2; } // appears second
```

### 2.5 Common Layout Patterns

#### Pattern 1: Centered Content (The Holy Grail of CSS)

```scss
.centered {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
}
```

#### Pattern 2: Navigation Bar

```scss
.navbar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0 24px;
  height: 64px;
  background: #333;
}

.navbar__logo {
  flex-shrink: 0;
}

.navbar__links {
  display: flex;
  gap: 24px;
  list-style: none;
}

.navbar__actions {
  display: flex;
  gap: 12px;
}
```

```
┌──────────────────────────────────────────┐
│ [Logo]    [Home] [About] [Contact]  [CTA]│
└──────────────────────────────────────────┘
```

#### Pattern 3: Card Grid

```scss
.card-grid {
  display: flex;
  flex-wrap: wrap;
  gap: 24px;
}

.card {
  flex: 1 1 300px; // grow, shrink, minimum 300px
  max-width: 100%;
}
```

#### Pattern 4: Sticky Footer

```scss
body {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

main {
  flex: 1; // grows to fill available space, pushing footer down
}

footer {
  flex-shrink: 0;
}
```

#### Pattern 5: Sidebar Layout

```scss
.layout {
  display: flex;
  min-height: 100vh;
}

.sidebar {
  flex: 0 0 250px; // fixed 250px width
}

.main-content {
  flex: 1; // takes remaining space
}
```

#### Pattern 6: Equal Height Columns

```scss
.columns {
  display: flex;
  gap: 24px;
}

.column {
  flex: 1; // equal width, equal height (stretch is default)
}
```

### 2.6 Responsive Design with Flexbox

Flexbox naturally lends itself to responsive design:

```scss
.card-container {
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
}

.card {
  flex: 1 1 100%; // mobile: full width

  @media (min-width: 768px) {
    flex: 1 1 calc(50% - 20px); // tablet: 2 columns
  }

  @media (min-width: 1024px) {
    flex: 1 1 calc(33.333% - 20px); // desktop: 3 columns
  }
}
```

**Mobile-first direction change:**

```scss
.hero {
  display: flex;
  flex-direction: column; // mobile: stacked
  gap: 24px;

  @media (min-width: 768px) {
    flex-direction: row; // desktop: side by side
  }
}

.hero__content { flex: 1; }
.hero__image { flex: 1; }
```

### 2.7 Flexbox vs Grid — When to Use Which

| Scenario | Flexbox | Grid |
|----------|---------|------|
| One-dimensional layout (row OR column) | **Yes** | Overkill |
| Two-dimensional layout (rows AND columns) | Hacky | **Yes** |
| Unknown number of items | **Yes** | Possible |
| Content-driven sizing | **Yes** | Possible |
| Layout-driven sizing | Possible | **Yes** |
| Alignment | **Excellent** | **Excellent** |
| Navigation bar | **Yes** | Possible |
| Card grid | Possible | **Yes** |
| Form layout | Possible | **Yes** |
| Centering content | **Yes** | **Yes** |

**Rule of thumb:**
- **Flexbox** = one direction at a time, content dictates layout
- **Grid** = two directions, layout dictates content placement

### 2.8 Practical Exercise: Navigation + Card Layout

Build this layout using Flexbox (SCSS):

```scss
// Navigation with logo, links and CTA
.nav {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px 32px;
  background: white;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);

  &__logo {
    font-size: 1.5rem;
    font-weight: 700;
    color: $primary;
  }

  &__links {
    display: flex;
    gap: 32px;
    list-style: none;

    @media (max-width: 768px) {
      display: none;
    }
  }

  &__link {
    color: #666;
    text-decoration: none;
    font-weight: 500;
    transition: color 0.2s;

    &:hover { color: $primary; }
    &--active { color: $primary; }
  }

  &__cta {
    @include button($primary);
    
    @media (max-width: 768px) {
      display: none;
    }
  }

  &__hamburger {
    display: none;
    @media (max-width: 768px) {
      display: block;
    }
  }
}

// Card grid
.cards {
  display: flex;
  flex-wrap: wrap;
  gap: 24px;
  padding: 32px;
  max-width: 1200px;
  margin: 0 auto;
}

.card {
  flex: 1 1 300px;
  background: white;
  border-radius: 12px;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
  transition: transform 0.2s, box-shadow 0.2s;

  &:hover {
    transform: translateY(-4px);
    box-shadow: 0 8px 24px rgba(0, 0, 0, 0.12);
  }

  &__image {
    width: 100%;
    height: 200px;
    object-fit: cover;
  }

  &__body {
    padding: 20px;
    display: flex;
    flex-direction: column;
    gap: 12px;
  }

  &__title {
    font-size: 1.25rem;
    font-weight: 600;
  }

  &__text {
    color: #666;
    line-height: 1.6;
    @include truncate(3);
  }

  &__footer {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 16px 20px;
    border-top: 1px solid #eee;
  }
}
```

---

> **BREAK (10 minutes)**

---

## Part 3: Tailwind CSS (75 min)

### 3.1 What Is Utility-First CSS?

Traditional CSS approaches write **semantic class names** that describe what an element *is*:

```html
<!-- Traditional / Semantic CSS -->
<button class="btn btn-primary btn-large">Click me</button>
```

```css
.btn { padding: 8px 16px; border: none; border-radius: 4px; cursor: pointer; }
.btn-primary { background: #3498db; color: white; }
.btn-large { padding: 12px 24px; font-size: 1.2rem; }
```

**Utility-first CSS** writes classes that describe what an element *looks like*:

```html
<!-- Utility-First (Tailwind) -->
<button class="bg-blue-500 text-white px-6 py-3 rounded-lg text-lg hover:bg-blue-600 cursor-pointer">
  Click me
</button>
```

Each class does exactly one thing. You compose them to build any design.

**The key insight:** You rarely write custom CSS. Instead, you apply pre-built utility classes directly in HTML.

### 3.2 Tailwind vs Traditional CSS vs Component Frameworks

| Aspect | Traditional CSS | Component Framework (Bootstrap/Materialize) | Tailwind CSS |
|--------|----------------|---------------------------------------------|-------------|
| Approach | Write custom CSS | Use predefined components | Compose utility classes |
| File naming | `.card`, `.hero` | `.btn`, `.card`, `.navbar` | `bg-blue-500`, `p-4`, `flex` |
| Customization | Full control | Override framework | Full control via utilities |
| Bundle size | Grows with project | Large (unused CSS) | Tiny (purges unused) |
| Learning curve | Know CSS | Learn component API | Learn utility names |
| Consistency | Varies | Built-in | Design system enforced |
| Unique designs | Easy | Fight the framework | Easy |
| Prototyping speed | Slow | Fast | **Fastest** |

**Why Tailwind is winning:**
1. **No context switching** — stay in HTML, no jumping to CSS files
2. **Design constraints** — spacing, colors, sizes from a consistent scale
3. **Tiny production bundles** — PurgeCSS removes unused utilities
4. **No naming fatigue** — no inventing `.card-wrapper-inner-content`
5. **Safe to change** — utilities are local, no global CSS side effects

### 3.3 Setup and Installation

#### Method 1: CDN (Development / Prototyping)

```html
<script src="https://cdn.tailwindcss.com"></script>
```

This is what we use in our ASP.NET MVC project for simplicity. Not recommended for production.

#### Method 2: npm (Production)

```bash
npm install -D tailwindcss
npx tailwindcss init
```

**tailwind.config.js:**
```javascript
module.exports = {
  content: [
    './Views/**/*.cshtml',
    './wwwroot/**/*.js',
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}
```

**Input CSS (src/input.css):**
```css
@tailwind base;
@tailwind components;
@tailwind utilities;
```

**Build command:**
```bash
npx tailwindcss -i ./src/input.css -o ./wwwroot/css/site.css --watch
```

#### Method 3: PostCSS Plugin

```bash
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p
```

### 3.4 Core Utility Classes

#### Spacing (Margin & Padding)

Tailwind uses a **4px base unit** scale:

| Class | Value |
|-------|-------|
| `p-0` | `0px` |
| `p-1` | `4px` (0.25rem) |
| `p-2` | `8px` (0.5rem) |
| `p-3` | `12px` (0.75rem) |
| `p-4` | `16px` (1rem) |
| `p-5` | `20px` (1.25rem) |
| `p-6` | `24px` (1.5rem) |
| `p-8` | `32px` (2rem) |
| `p-10` | `40px` (2.5rem) |
| `p-12` | `48px` (3rem) |
| `p-16` | `64px` (4rem) |
| `p-20` | `80px` (5rem) |

**Directional prefixes:**

| Prefix | Meaning |
|--------|---------|
| `p-` | all sides |
| `px-` | horizontal (left + right) |
| `py-` | vertical (top + bottom) |
| `pt-` | top |
| `pr-` | right |
| `pb-` | bottom |
| `pl-` | left |

Same pattern for margin: `m-`, `mx-`, `my-`, `mt-`, `mr-`, `mb-`, `ml-`

**Negative margins:** prefix with `-`: `-mt-4` = `margin-top: -16px`

**Auto margins:** `mx-auto` = `margin-left: auto; margin-right: auto`

#### Typography

```html
<!-- Font size -->
<p class="text-xs">12px</p>
<p class="text-sm">14px</p>
<p class="text-base">16px (default)</p>
<p class="text-lg">18px</p>
<p class="text-xl">20px</p>
<p class="text-2xl">24px</p>
<p class="text-3xl">30px</p>
<p class="text-4xl">36px</p>
<p class="text-5xl">48px</p>

<!-- Font weight -->
<p class="font-light">300</p>
<p class="font-normal">400</p>
<p class="font-medium">500</p>
<p class="font-semibold">600</p>
<p class="font-bold">700</p>

<!-- Text alignment -->
<p class="text-left">Left</p>
<p class="text-center">Center</p>
<p class="text-right">Right</p>

<!-- Text color -->
<p class="text-gray-500">Gray text</p>
<p class="text-blue-600">Blue text</p>
<p class="text-red-500">Red text</p>

<!-- Line height -->
<p class="leading-none">1</p>
<p class="leading-tight">1.25</p>
<p class="leading-normal">1.5</p>
<p class="leading-relaxed">1.625</p>
<p class="leading-loose">2</p>

<!-- Letter spacing -->
<p class="tracking-tight">-0.05em</p>
<p class="tracking-normal">0</p>
<p class="tracking-wide">0.025em</p>

<!-- Text decoration -->
<p class="underline">Underlined</p>
<p class="line-through">Strikethrough</p>
<p class="no-underline">No underline</p>
```

#### Colors

Tailwind provides a full color palette with 10 shades each:

```
{color}-50   → lightest
{color}-100
{color}-200
{color}-300
{color}-400
{color}-500  → base / medium
{color}-600
{color}-700
{color}-800
{color}-900
{color}-950  → darkest
```

Available colors: `slate`, `gray`, `zinc`, `neutral`, `stone`, `red`, `orange`, `amber`, `yellow`, `lime`, `green`, `emerald`, `teal`, `cyan`, `sky`, `blue`, `indigo`, `violet`, `purple`, `fuchsia`, `pink`, `rose`

**Usage:**
```html
<div class="bg-blue-500">Background</div>
<p class="text-gray-700">Text color</p>
<div class="border border-red-300">Border color</div>
<div class="ring-2 ring-green-500">Ring (outline) color</div>
```

#### Width & Height

```html
<!-- Fixed widths -->
<div class="w-64">256px</div>
<div class="w-full">100%</div>
<div class="w-screen">100vw</div>
<div class="w-1/2">50%</div>
<div class="w-1/3">33.333%</div>

<!-- Max width (for containers) -->
<div class="max-w-sm">384px</div>
<div class="max-w-md">448px</div>
<div class="max-w-lg">512px</div>
<div class="max-w-xl">576px</div>
<div class="max-w-2xl">672px</div>
<div class="max-w-7xl">1280px</div>

<!-- Height -->
<div class="h-16">64px</div>
<div class="h-screen">100vh</div>
<div class="min-h-screen">min-height: 100vh</div>
```

#### Flexbox in Tailwind

Everything we learned about Flexbox maps directly to Tailwind classes:

```html
<!-- Container -->
<div class="flex">                    <!-- display: flex -->
<div class="flex flex-col">           <!-- flex-direction: column -->
<div class="flex flex-row-reverse">   <!-- flex-direction: row-reverse -->
<div class="flex flex-wrap">          <!-- flex-wrap: wrap -->

<!-- Justify content -->
<div class="flex justify-start">      <!-- justify-content: flex-start -->
<div class="flex justify-center">     <!-- justify-content: center -->
<div class="flex justify-end">        <!-- justify-content: flex-end -->
<div class="flex justify-between">    <!-- justify-content: space-between -->
<div class="flex justify-around">     <!-- justify-content: space-around -->
<div class="flex justify-evenly">     <!-- justify-content: space-evenly -->

<!-- Align items -->
<div class="flex items-start">        <!-- align-items: flex-start -->
<div class="flex items-center">       <!-- align-items: center -->
<div class="flex items-end">          <!-- align-items: flex-end -->
<div class="flex items-stretch">      <!-- align-items: stretch -->

<!-- Gap -->
<div class="flex gap-4">              <!-- gap: 16px -->
<div class="flex gap-x-4 gap-y-2">   <!-- column-gap: 16px; row-gap: 8px -->

<!-- Items -->
<div class="flex-1">                  <!-- flex: 1 1 0% -->
<div class="flex-auto">               <!-- flex: 1 1 auto -->
<div class="flex-none">               <!-- flex: none -->
<div class="flex-grow">               <!-- flex-grow: 1 -->
<div class="flex-shrink-0">           <!-- flex-shrink: 0 -->
<div class="self-center">             <!-- align-self: center -->
<div class="order-first">             <!-- order: -9999 -->
<div class="order-last">              <!-- order: 9999 -->
```

**Practical: Centering with Tailwind:**

```html
<!-- Center anything horizontally and vertically -->
<div class="flex justify-center items-center min-h-screen">
  <div>I'm perfectly centered!</div>
</div>
```

#### Borders & Rounded Corners

```html
<!-- Border -->
<div class="border">1px solid</div>
<div class="border-2">2px</div>
<div class="border-4">4px</div>
<div class="border-t">top only</div>
<div class="border-b-2 border-blue-500">bottom 2px blue</div>

<!-- Rounded -->
<div class="rounded">4px</div>
<div class="rounded-md">6px</div>
<div class="rounded-lg">8px</div>
<div class="rounded-xl">12px</div>
<div class="rounded-2xl">16px</div>
<div class="rounded-full">9999px (circle)</div>
```

#### Shadows

```html
<div class="shadow-sm">small shadow</div>
<div class="shadow">default shadow</div>
<div class="shadow-md">medium shadow</div>
<div class="shadow-lg">large shadow</div>
<div class="shadow-xl">extra large shadow</div>
<div class="shadow-2xl">huge shadow</div>
```

### 3.5 Responsive Design in Tailwind

Tailwind is **mobile-first**. Breakpoint prefixes apply styles at that width **and above**:

| Prefix | Min-width | CSS |
|--------|-----------|-----|
| (none) | 0px | Default (mobile) |
| `sm:` | 640px | `@media (min-width: 640px)` |
| `md:` | 768px | `@media (min-width: 768px)` |
| `lg:` | 1024px | `@media (min-width: 1024px)` |
| `xl:` | 1280px | `@media (min-width: 1280px)` |
| `2xl:` | 1536px | `@media (min-width: 1536px)` |

**Example: Responsive grid**

```html
<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
  <div class="bg-white p-6 rounded-lg shadow">Card 1</div>
  <div class="bg-white p-6 rounded-lg shadow">Card 2</div>
  <div class="bg-white p-6 rounded-lg shadow">Card 3</div>
</div>
```

- **Mobile** (< 768px): 1 column
- **Tablet** (768px+): 2 columns
- **Desktop** (1024px+): 3 columns

**Example: Responsive navigation**

```html
<nav class="flex flex-col md:flex-row md:items-center md:justify-between p-4">
  <div class="text-xl font-bold">Logo</div>
  <div class="hidden md:flex space-x-4">
    <a href="#">Home</a>
    <a href="#">About</a>
    <a href="#">Contact</a>
  </div>
</nav>
```

**Example: Responsive padding and font size**

```html
<h1 class="text-2xl md:text-4xl lg:text-6xl p-4 md:p-8 lg:p-16">
  Responsive Heading
</h1>
```

### 3.6 State Variants (hover, focus, active)

Tailwind handles pseudo-classes via prefixes:

```html
<!-- Hover -->
<button class="bg-blue-500 hover:bg-blue-700 text-white">
  Hover me
</button>

<!-- Focus -->
<input class="border focus:border-blue-500 focus:ring-2 focus:ring-blue-200 outline-none" />

<!-- Active -->
<button class="bg-green-500 active:bg-green-700">
  Press me
</button>

<!-- Group hover (parent hover affects child) -->
<div class="group p-4 hover:bg-gray-100 rounded-lg">
  <h3 class="group-hover:text-blue-500">Title</h3>
  <p class="group-hover:text-gray-700">Description</p>
</div>

<!-- First/Last child -->
<ul>
  <li class="first:pt-0 last:pb-0 py-4 border-b last:border-0">Item</li>
</ul>

<!-- Odd/Even -->
<tr class="odd:bg-white even:bg-gray-50">

<!-- Disabled -->
<button class="disabled:opacity-50 disabled:cursor-not-allowed" disabled>
  Disabled
</button>

<!-- Placeholder -->
<input class="placeholder:text-gray-400 placeholder:italic" placeholder="Search..." />
```

**Combining variants:**

```html
<button class="md:hover:bg-blue-700">
  Only hover effect on md+ screens
</button>
```

### 3.7 Dark Mode

Tailwind supports dark mode out of the box:

**Strategy 1: Media query (follows OS setting)**

```javascript
// tailwind.config.js
module.exports = {
  darkMode: 'media', // default
}
```

**Strategy 2: Class-based (manual toggle)**

```javascript
// tailwind.config.js
module.exports = {
  darkMode: 'class',
}
```

```html
<!-- Add 'dark' class to html or body to enable -->
<html class="dark">
  <body class="bg-white dark:bg-gray-900 text-gray-900 dark:text-gray-100">
    <div class="bg-gray-100 dark:bg-gray-800 p-6 rounded-lg">
      <h2 class="text-gray-800 dark:text-gray-200">Dark Mode Card</h2>
      <p class="text-gray-600 dark:text-gray-400">Content here</p>
    </div>
  </body>
</html>
```

**Toggle with JavaScript:**

```javascript
document.getElementById('theme-toggle').addEventListener('click', () => {
  document.documentElement.classList.toggle('dark');
});
```

### 3.8 Customizing Tailwind (tailwind.config.js)

The config file lets you customize every aspect of the design system:

```javascript
module.exports = {
  content: ['./Views/**/*.cshtml'],
  theme: {
    // Override defaults entirely
    screens: {
      'sm': '576px',
      'md': '768px',
      'lg': '992px',
      'xl': '1200px',
    },
    
    // Extend (add to) defaults
    extend: {
      colors: {
        'brand': {
          50: '#eff6ff',
          100: '#dbeafe',
          500: '#3b82f6',
          600: '#2563eb',
          700: '#1d4ed8',
          900: '#1e3a8a',
        },
        'wsb': '#e74c3c',
      },
      fontFamily: {
        'sans': ['Inter', 'system-ui', 'sans-serif'],
        'display': ['Playfair Display', 'serif'],
      },
      spacing: {
        '128': '32rem',
        '144': '36rem',
      },
      borderRadius: {
        '4xl': '2rem',
      },
      animation: {
        'fade-in': 'fadeIn 0.5s ease-in-out',
        'slide-up': 'slideUp 0.3s ease-out',
      },
      keyframes: {
        fadeIn: {
          '0%': { opacity: '0' },
          '100%': { opacity: '1' },
        },
        slideUp: {
          '0%': { transform: 'translateY(20px)', opacity: '0' },
          '100%': { transform: 'translateY(0)', opacity: '1' },
        },
      },
    },
  },
  plugins: [
    require('@tailwindcss/forms'),
    require('@tailwindcss/typography'),
  ],
}
```

**Usage of custom values:**

```html
<div class="bg-brand-500 text-white font-display">
  Custom branded heading
</div>
<div class="bg-wsb rounded-4xl animate-fade-in">
  WSB colored content
</div>
```

**Arbitrary values (one-off overrides):**

```html
<div class="w-[137px] h-[42px] bg-[#1da1f2] text-[13px] top-[117px]">
  Exact values when needed
</div>
```

### 3.9 Extracting Components (@apply)

When utility chains get repetitive, extract them:

**Option 1: @apply in CSS (for repeated patterns)**

```css
@tailwind base;
@tailwind components;
@tailwind utilities;

@layer components {
  .btn {
    @apply px-4 py-2 rounded-lg font-medium transition-colors duration-200 cursor-pointer;
  }
  
  .btn-primary {
    @apply btn bg-blue-500 text-white hover:bg-blue-600;
  }
  
  .btn-secondary {
    @apply btn bg-gray-200 text-gray-800 hover:bg-gray-300;
  }
  
  .btn-danger {
    @apply btn bg-red-500 text-white hover:bg-red-600;
  }
  
  .card {
    @apply bg-white rounded-xl shadow-md p-6 border border-gray-100;
  }
  
  .input {
    @apply w-full px-4 py-2 border border-gray-300 rounded-lg
           focus:border-blue-500 focus:ring-2 focus:ring-blue-200 outline-none
           transition-all duration-200;
  }
}
```

**Option 2: Template partials / components (preferred in frameworks)**

In ASP.NET MVC, use Partial Views:

```html
<!-- _Button.cshtml -->
<a href="@ViewBag.Href" 
   class="inline-block bg-orange-500 hover:bg-orange-600 text-white text-lg font-medium px-8 py-3 rounded-lg shadow-lg hover:shadow-xl transition-all duration-200">
    @ViewBag.Text
</a>
```

**The Tailwind team recommends components/partials over @apply** because @apply defeats the purpose of utility-first CSS.

### 3.10 Practical Exercise: Convert Materialize to Tailwind

This is exactly what we did in our project! Let's walk through the conversion step by step.

**Original Materialize code:**

```html
<nav class="light-blue lighten-1" role="navigation">
  <div class="nav-wrapper container">
    <a id="logo-container" href="#" class="brand-logo">WSB-NLU MVC</a>
    <ul class="right hide-on-med-and-down">
      <li><a href="/">Strona główna</a></li>
      <li><a href="/About">O firmie</a></li>
      <li><a href="/Contact">Kontakt</a></li>
    </ul>
  </div>
</nav>
```

**Converted to Tailwind:**

```html
<nav class="bg-sky-400" role="navigation">
  <div class="max-w-7xl mx-auto px-4 py-3 flex items-center justify-between">
    <a href="#" class="text-xl font-bold text-white no-underline">WSB-NLU MVC</a>
    <ul class="hidden md:flex space-x-6 list-none m-0 p-0">
      <li><a class="text-white hover:text-sky-100 transition no-underline" href="/">Strona główna</a></li>
      <li><a class="text-white hover:text-sky-100 transition no-underline" href="/About">O firmie</a></li>
      <li><a class="text-white hover:text-sky-100 transition no-underline" href="/Contact">Kontakt</a></li>
    </ul>
  </div>
</nav>
```

**Mapping table — Materialize → Tailwind:**

| Materialize Class | Tailwind Equivalent | Purpose |
|------------------|--------------------|---------| 
| `container` | `max-w-7xl mx-auto px-4` | Centered container |
| `row` | `flex flex-wrap` or `grid` | Row of columns |
| `col s12` | `w-full` | Full-width column |
| `col s12 m4` | `w-full md:w-1/3` | Responsive column |
| `col s12 m6` | `w-full md:w-1/2` | Half-width on medium |
| `col l6 s12` | `w-full lg:w-1/2` | Half-width on large |
| `center` | `text-center` | Center text |
| `light` | `font-light` | Light font weight |
| `light-blue lighten-1` | `bg-sky-400` | Blue background |
| `orange-text` | `text-orange-500` | Orange text |
| `white-text` | `text-white` | White text |
| `grey-text text-lighten-4` | `text-gray-300` | Light gray text |
| `btn-large` | `px-8 py-3 text-lg rounded-lg` | Large button |
| `waves-effect waves-light orange` | `bg-orange-500 hover:bg-orange-600 transition` | Animated button |
| `page-footer orange` | `bg-orange-500 mt-auto` | Footer |
| `hide-on-med-and-down` | `hidden md:flex` | Hide on mobile |
| `brand-logo` | `text-xl font-bold text-white` | Logo styling |
| `nav-wrapper` | `flex items-center justify-between` | Nav layout |
| `section` | `py-12` or `py-16` | Section spacing |
| `icon-block` | `text-center` | Icon card |
| `header` | `font-bold` or `font-semibold` | Heading style |
| `right` | `ml-auto` or `flex justify-end` | Float right |
| `sidenav` | Mobile menu with `hidden md:hidden` toggle | Mobile nav |

---

## Part 4: Putting It All Together (15 min)

### 4.1 SCSS + Tailwind: Can They Coexist?

Yes! You can use SCSS as the preprocessor that generates your CSS, even with Tailwind:

```scss
// styles.scss
@tailwind base;
@tailwind components;
@tailwind utilities;

$brand-spacing: 24px;

@layer components {
  .custom-card {
    @apply bg-white rounded-xl shadow-md;
    padding: $brand-spacing; // SCSS variable
    
    &__title {
      @apply text-xl font-bold text-gray-800;
      margin-bottom: $brand-spacing / 2;
    }
    
    &:hover {
      @apply shadow-xl;
      transform: translateY(-2px);
    }
  }
}
```

**When this makes sense:**
- You have existing SCSS that you're gradually migrating to Tailwind
- You need SCSS features (loops, mixins) to generate utility classes
- Your team is comfortable with SCSS and wants to keep some patterns

**When to use pure Tailwind:**
- New projects with no legacy CSS
- Teams fully committed to utility-first approach
- When you want maximum build speed (no Sass compilation step)

### 4.2 Choosing the Right Tool

```
                    ┌─────────────────────────────┐
                    │ Do you need custom design?   │
                    └─────────────┬───────────────┘
                          ┌───────┴───────┐
                        Yes               No
                          │                │
                   ┌──────┴──────┐   Use Bootstrap/
                   │ Large team? │   Materialize
                   └──────┬──────┘
                    ┌──────┴──────┐
                  Yes             No
                    │              │
             ┌──────┴──────┐   Use Tailwind
             │ Complex     │   (fastest for
             │ theming?    │    solo/small teams)
             └──────┬──────┘
              ┌─────┴─────┐
            Yes           No
              │            │
        SCSS + Design    Tailwind
        Tokens           (with config)
```

**Summary:**

| Tool | Best For |
|------|----------|
| **SCSS** | Complex theming, large codebases, design tokens, team conventions |
| **Flexbox** | Any layout — it's not a tool choice, it's foundational CSS |
| **Tailwind** | Rapid development, consistent designs, small-to-medium projects |
| **SCSS + Tailwind** | Migration projects, complex + rapid needs |

### 4.3 Key Takeaways

1. **SCSS** extends CSS with variables, nesting, mixins, and functions — making stylesheets maintainable and DRY
2. **Flexbox** is the foundation of modern CSS layout — understand it deeply regardless of which framework you use
3. **Tailwind CSS** is a utility-first framework that enforces design consistency and dramatically speeds up development
4. **All three are complementary** — SCSS is a preprocessor, Flexbox is a layout model, Tailwind is a utility framework
5. **The best tool depends on context** — project size, team, timeline, and design requirements

---

## Homework

1. **SCSS Exercise:** Create a complete `_variables.scss` and `_mixins.scss` for the Company project with at least 5 variables and 3 mixins
2. **Flexbox Exercise:** Build a responsive product card grid (3 columns desktop, 2 tablet, 1 mobile) using pure Flexbox
3. **Tailwind Exercise:** Convert the footer section to include a 4-column responsive grid with actual content and hover effects
4. **Bonus:** Add dark mode support to the homepage using Tailwind's `dark:` variant

---

## Resources

- [Sass Documentation](https://sass-lang.com/documentation/)
- [CSS-Tricks: A Complete Guide to Flexbox](https://css-tricks.com/snippets/css/a-guide-to-flexbox/)
- [Flexbox Froggy (Interactive Game)](https://flexboxfroggy.com/)
- [Tailwind CSS Documentation](https://tailwindcss.com/docs)
- [Tailwind Play (Online Playground)](https://play.tailwindcss.com/)
- [Tailwind UI (Component Examples)](https://tailwindui.com/)

---

*Lecture prepared for WSB-NLU Internet Technologies course*
*Last updated: February 2026*
