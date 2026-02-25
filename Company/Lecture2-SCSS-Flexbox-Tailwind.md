# Lecture 2: SCSS, Flexbox & Tailwind CSS — Building a Modern Portal

**Duration:** 4 hours (with breaks)  
**Prerequisites:** HTML basics, ASP.NET MVC project from Lecture 1  
**What we build:** A complete, modern portal with hero sections, product gallery, contact form, and style guide — all using Tailwind CSS, Flexbox layouts, and SCSS theory applied in practice.

---

## Table of Contents

| # | Section | Duration | What We Cover |
|---|---------|----------|---------------|
| 1 | [SCSS Fundamentals](#part-1-scss-fundamentals-75-min) | 75 min | Theory + live SCSS coding |
| 2 | [Flexbox Deep Dive](#part-2-flexbox-deep-dive-75-min) | 75 min | Navigation, cards, product grid |
| 3 | [Tailwind CSS — Theory](#part-3-tailwind-css--theory-60-min) | 60 min | Utility-first concepts, classes, responsive, dark mode, config |
| 4 | [Tailwind CSS — In Practice](#part-4-tailwind-css--in-practice-50-min) | 50 min | Build all pages from scratch in our project |
| 5 | [Integration & Wrapup](#part-5-integration--wrapup-15-min) | 15 min | SCSS + Tailwind, best practices |

---

## Part 1: SCSS Fundamentals (75 min)

### 1.1 — What Problem Does SCSS Solve?

Open any large CSS file. You'll see:
- Colors copy-pasted everywhere (`#3498db` appears 47 times)
- Long, flat selectors that don't show hierarchy
- No way to reuse patterns (the same 5-line shadow appears on 12 elements)
- No math (`width: 33.333%` — where does that come from?)

**SCSS (Sassy CSS)** is a preprocessor that adds programming features to CSS:

```
SCSS (.scss file)  →  Sass Compiler  →  CSS (.css file)  →  Browser
```

The browser never sees SCSS — it only sees the compiled CSS output.

> **Key insight:** SCSS is a *superset* of CSS. Any valid CSS file is already valid SCSS. You can adopt it incrementally.

### 1.2 — Variables: Your Design Tokens

Instead of scattering raw values everywhere, define them once:

```scss
// _variables.scss — the single source of truth

// Colors
$primary:     #7c3aed;  // violet-600
$secondary:   #4f46e5;  // indigo-600
$accent:      #ec4899;  // pink-500
$gray-900:    #111827;
$gray-50:     #f9fafb;

// Spacing (4px base unit — same as Tailwind!)
$space-unit:  4px;
$space-1:     $space-unit * 1;   //  4px
$space-2:     $space-unit * 2;   //  8px
$space-4:     $space-unit * 4;   // 16px
$space-6:     $space-unit * 6;   // 24px
$space-8:     $space-unit * 8;   // 32px

// Breakpoints
$bp-sm:  640px;
$bp-md:  768px;
$bp-lg:  1024px;
$bp-xl:  1280px;

// Typography
$font-sans: 'Inter', system-ui, sans-serif;
$text-base: 16px;
$text-lg:   18px;
$text-4xl:  36px;
```

**Usage:**

```scss
body {
  font-family: $font-sans;
  font-size: $text-base;
  color: $gray-900;
  background: $gray-50;
}

.btn-primary {
  background: $primary;
  padding: $space-2 $space-4;
}
```

**`!default` flag** — essential for configurable libraries:

```scss
$primary: #7c3aed !default;  // "use this only if $primary isn't already defined"
```

**SCSS vars vs CSS custom properties:**

| | SCSS `$var` | CSS `--var` |
|---|---|---|
| When resolved | Compile time | Runtime |
| Can change with JS? | No | Yes |
| Responsive to media queries? | No | Yes |
| Best for | Build-time tokens | Dynamic theming |

### 1.3 — Nesting: Mirror Your HTML

Instead of flat, repetitive selectors:

```css
/* Plain CSS — repetitive */
.navbar { background: #111827; }
.navbar ul { list-style: none; display: flex; }
.navbar ul li a { color: white; text-decoration: none; }
.navbar ul li a:hover { color: #a78bfa; }
```

Write nested SCSS that mirrors the HTML tree:

```scss
// SCSS — nested, readable
.navbar {
  background: $gray-900;

  ul {
    list-style: none;
    display: flex;

    li a {
      color: white;
      text-decoration: none;

      &:hover {
        color: lighten($primary, 20%);
      }
    }
  }
}
```

**The `&` (parent selector)** is the most powerful character in SCSS:

```scss
.card {
  background: white;
  border-radius: 24px;

  // Pseudo-classes
  &:hover { box-shadow: 0 20px 40px rgba(0,0,0,0.1); }
  &:focus-within { border-color: $primary; }

  // Pseudo-elements
  &::before { content: ''; /* decorative element */ }

  // BEM modifiers
  &--featured { border: 2px solid $primary; }
  &--sold-out { opacity: 0.5; }

  // BEM elements
  &__image { aspect-ratio: 1; }
  &__body { padding: $space-6; }
  &__title { font-size: $text-lg; font-weight: 700; }
  &__price { color: $primary; font-weight: 800; }

  // Context: parent theme changes child
  .dark-mode & { background: $gray-900; }
}
```

> **The Inception Rule:** Never nest more than 3 levels deep. Deep nesting creates overly specific selectors that are hard to override.

### 1.4 — Partials & File Organization

Split styles into small, focused files:

```
scss/
├── abstracts/
│   ├── _variables.scss      // Design tokens
│   ├── _mixins.scss         // Reusable patterns
│   └── _functions.scss      // Helper functions
├── base/
│   ├── _reset.scss          // CSS reset
│   └── _typography.scss     // Font rules
├── components/
│   ├── _buttons.scss
│   ├── _cards.scss
│   └── _navbar.scss
├── layout/
│   ├── _header.scss
│   ├── _footer.scss
│   └── _grid.scss
├── pages/
│   ├── _home.scss
│   └── _contact.scss
└── main.scss                // Entry point — imports everything
```

**main.scss** pulls it all together:

```scss
@use 'abstracts/variables' as *;
@use 'abstracts/mixins' as *;

@use 'base/reset';
@use 'base/typography';

@use 'components/buttons';
@use 'components/cards';
@use 'components/navbar';

@use 'layout/header';
@use 'layout/footer';

@use 'pages/home';
@use 'pages/contact';
```

> **Note:** `@import` is deprecated. Use `@use` (with namespaces) and `@forward` (to re-export).

### 1.5 — Mixins: Reusable CSS Blocks

Mixins are like functions that output CSS:

```scss
// _mixins.scss

@mixin flex-center {
  display: flex;
  justify-content: center;
  align-items: center;
}

@mixin respond-to($breakpoint) {
  @if $breakpoint == 'md' {
    @media (min-width: $bp-md) { @content; }
  } @else if $breakpoint == 'lg' {
    @media (min-width: $bp-lg) { @content; }
  }
}

@mixin gradient-text($from, $to) {
  background: linear-gradient(to right, $from, $to);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

@mixin button($bg, $color: white) {
  background: $bg;
  color: $color;
  border: none;
  border-radius: 12px;
  padding: $space-2 $space-6;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;

  &:hover {
    background: darken($bg, 10%);
    transform: translateY(-1px);
  }
}
```

**Usage in our project:**

```scss
.hero {
  @include flex-center;
  min-height: 100vh;
}

.hero__title {
  @include gradient-text($primary, $secondary);
  font-size: $text-4xl;
  font-weight: 800;
}

.container {
  width: 100%;
  max-width: 1280px;
  margin: 0 auto;
  padding: 0 $space-6;

  @include respond-to('md') {
    padding: 0 $space-8;
  }
}

.btn-primary { @include button($primary); }
.btn-danger  { @include button(#ef4444); }
.btn-success { @include button(#10b981); }
```

### 1.6 — Extend / Inheritance

`@extend` lets selectors share identical styles:

```scss
%card-base {
  background: white;
  border-radius: 24px;
  border: 1px solid #e5e7eb;
  overflow: hidden;
  transition: all 0.3s ease;
}

.product-card {
  @extend %card-base;
  // product-specific additions
}

.blog-card {
  @extend %card-base;
  // blog-specific additions
}
```

**`%placeholder` selectors** only emit CSS when extended — they're invisible by default.

| | `@extend` | `@mixin` |
|---|---|---|
| Parameters? | No | Yes |
| CSS output | Grouped selectors (smaller) | Duplicated declarations |
| Use when | Styles are identical | Styles are parameterized |

### 1.7 — Functions & Operators

```scss
@use 'sass:math';

@function rem($px) {
  @return math.div($px, 16) * 1rem;
}

@function contrast-color($bg) {
  $luminance: red($bg) * 0.299 + green($bg) * 0.587 + blue($bg) * 0.114;
  @return if($luminance > 128, #000, #fff);
}

// Usage
h1 { font-size: rem(48); }       // 3rem
h2 { font-size: rem(36); }       // 2.25rem
body { font-size: rem(16); }     // 1rem
```

**Built-in color functions:**

```scss
$base: #7c3aed;

.element {
  background: $base;                           // violet-600
  border: 1px solid darken($base, 15%);        // darker
  color: lighten($base, 40%);                  // lighter
  box-shadow: 0 4px 12px rgba($base, 0.3);     // semi-transparent
}
```

### 1.8 — Control Flow: Loops & Conditionals

**Generate utility classes (this is literally how Tailwind works under the hood):**

```scss
// Generate spacing utilities
@for $i from 0 through 16 {
  .p-#{$i}  { padding: $space-unit * $i; }
  .m-#{$i}  { margin: $space-unit * $i; }
  .mt-#{$i} { margin-top: $space-unit * $i; }
  .mb-#{$i} { margin-bottom: $space-unit * $i; }
}

// Generate color utilities from a map
$colors: (
  'violet': $primary,
  'indigo': $secondary,
  'pink':   $accent,
  'gray':   $gray-900,
);

@each $name, $color in $colors {
  .bg-#{$name} { background-color: $color; }
  .text-#{$name} { color: $color; }
  .border-#{$name} { border-color: $color; }

  .btn-#{$name} {
    @include button($color);
  }
}
```

> **Connecting the dots:** This is exactly what Tailwind does at build time — generates thousands of utility classes from a config object. SCSS shows you the *mechanism*, Tailwind gives you the *product*.

---

> **BREAK — 10 minutes** ☕

---

## Part 2: Flexbox Deep Dive (75 min)

### 2.1 — The Two Axes

Every flex container has two axes:

```
flex-direction: row (default)
┌────────────────────────────────────┐
│  ─────────→ MAIN AXIS              │
│  │                                 │
│  │ CROSS                           │
│  ↓ AXIS     [Item 1] [Item 2] [3] │
│                                    │
└────────────────────────────────────┘

flex-direction: column
┌──────────────┐
│  │ MAIN AXIS │
│  ↓           │
│  [Item 1]    │
│  [Item 2]    │
│  [Item 3]    │
│              │
│ ──→ CROSS   │
└──────────────┘
```

Everything in Flexbox relates to these two axes.

### 2.2 — Container Properties

Set `display: flex` on the parent — children become flex items:

```html
<nav class="flex items-center justify-between px-6 py-4">
  <a href="/">Logo</a>
  <ul class="flex gap-6">
    <li><a href="#">Home</a></li>
    <li><a href="#">About</a></li>
  </ul>
  <button>Contact</button>
</nav>
```

**justify-content** — main axis alignment:

```
flex-start:      |[A][B][C]                    |
center:          |          [A][B][C]          |
flex-end:        |                    [A][B][C]|
space-between:   |[A]         [B]         [C]  |
space-around:    |  [A]     [B]     [C]        |
space-evenly:    |    [A]    [B]    [C]        |
```

**align-items** — cross axis alignment:

```
stretch (default)    flex-start       center          flex-end
┌──────────────┐    ┌──────────────┐ ┌──────────────┐ ┌──────────────┐
│┌──┐┌────┐┌─┐│    │[A] [BB] [C]  │ │              │ │              │
││A ││ BB ││C││    │              │ │ [A] [BB] [C] │ │              │
││  ││    ││ ││    │              │ │              │ │[A] [BB] [C]  │
│└──┘└────┘└─┘│    └──────────────┘ └──────────────┘ └──────────────┘
└──────────────┘
```

**flex-wrap** — what happens when items don't fit:

```
nowrap (default): items shrink or overflow
┌─────────────────────────────┐
│[1][2][3][4][5][6][7][8][9]  │
└─────────────────────────────┘

wrap: items flow to next line
┌─────────────────────────────┐
│[1]  [2]  [3]  [4]  [5]     │
│[6]  [7]  [8]  [9]          │
└─────────────────────────────┘
```

**gap** — modern spacing (replaces margin hacks):

```css
.container {
  display: flex;
  gap: 24px;        /* equal spacing */
  row-gap: 16px;    /* or separate axes */
  column-gap: 24px;
}
```

### 2.3 — Item Properties

**flex-grow / flex-shrink / flex-basis** — the flex shorthand:

```css
.item {
  /* flex: grow shrink basis */
  flex: 0 1 auto;     /* default: don't grow, can shrink, auto size */
  flex: 1;            /* grow equally, shrink, basis 0 (equal columns) */
  flex: 1 1 300px;    /* grow from 300px, can shrink */
  flex: none;         /* rigid: don't grow, don't shrink */
  flex: 0 0 250px;    /* fixed 250px — perfect for sidebars */
}
```

**align-self** — override alignment for one item:

```css
.container { display: flex; align-items: flex-start; }
.special   { align-self: center; }  /* just this one centers */
```

**order** — visual reordering without changing HTML:

```css
.first-visual  { order: -1; }  /* moves to front */
.last-visual   { order: 999; } /* moves to end */
```

### 2.4 — Building Our Navigation Bar

**In our `_Layout.cshtml`, the navbar uses these Flexbox patterns:**

```html
<!-- Outer: logo left, links right — space-between -->
<div class="flex items-center justify-between">
    
    <!-- Logo: flex + gap for icon + text -->
    <a class="flex items-center gap-2">
        <div class="w-9 h-9 rounded-xl flex items-center justify-center">T</div>
        <span>TechStore</span>
    </a>

    <!-- Desktop links: hidden on mobile, flex row with gap -->
    <ul class="hidden md:flex items-center gap-1">
        <li><a>Home</a></li>
        <li><a>About</a></li>
        <li><a>Products</a></li>
    </ul>
</div>
```

Flexbox properties at work:
- `flex items-center justify-between` → horizontal layout, vertically centered, space between logo and links
- `flex items-center gap-2` → logo icon and text side by side with 8px gap
- `hidden md:flex` → hidden on mobile (no Flexbox), shown as flex on `md:` and up
- `gap-1` → 4px spacing between nav links

### 2.5 — Building the Product Grid

**In `Products.cshtml`, we use Flexbox wrapping instead of CSS Grid:**

```html
<!-- The magic container -->
<div class="flex flex-wrap gap-6">

    <!-- Each card: grows, shrinks, minimum 280px -->
    <div class="flex-1 min-w-[280px] max-w-sm">
        <!-- Card content... -->
        
        <!-- Card body uses column flex for vertical layout -->
        <div class="flex flex-col gap-3">
            <h3>Product Name</h3>
            <p>Description</p>
            
            <!-- Footer pushed to bottom with mt-auto -->
            <div class="flex items-center justify-between mt-auto">
                <span>$249</span>
                <button>Add to Cart</button>
            </div>
        </div>
    </div>
</div>
```

**How the responsive grid works WITHOUT media queries:**

1. `flex-wrap: wrap` — items can flow to the next line
2. `flex: 1 1 280px` (via `flex-1 min-w-[280px]`) — each card starts at 280px
3. When the container is 900px wide: 3 cards fit → 3 columns
4. When the container is 600px wide: 2 cards fit → 2 columns
5. When the container is 350px wide: 1 card fits → 1 column

This is **intrinsic responsive design** — the layout adapts based on available space, not arbitrary breakpoints.

### 2.6 — Common Layout Patterns

**Pattern 1: Sticky Footer** (used in our `_Layout.cshtml`)

```css
body {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}
main { flex-grow: 1; }    /* takes all available space */
footer { flex-shrink: 0; } /* never shrinks */
```

In Tailwind: `body class="flex flex-col min-h-screen"` + `main class="flex-grow"`

**Pattern 2: Centering** (used in hero section)

```css
.hero {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
}
```

In Tailwind: `class="flex justify-center items-center min-h-screen"`

**Pattern 3: Sidebar** 

```css
.layout { display: flex; }
.sidebar { flex: 0 0 250px; }     /* fixed width */
.content { flex: 1; }             /* takes the rest */
```

**Pattern 4: Card with Footer Pushed Down**

```css
.card { display: flex; flex-direction: column; }
.card__body { flex: 1; }           /* grows */
.card__footer { margin-top: auto; } /* pushed to bottom */
```

### 2.7 — Flexbox vs Grid

| Use Case | Flexbox | Grid |
|----------|---------|------|
| Navbar | Yes | Overkill |
| Card list (unknown count) | Yes | Also good |
| Centering | Yes | Yes |
| Two-dimensional dashboard | Hacky | Yes |
| Form layout | Possible | Better |
| Holy Grail layout | Possible | Better |

**Rule:** Flexbox = one dimension at a time, content drives layout. Grid = two dimensions, layout drives content.

---

> **BREAK — 10 minutes** ☕

---

## Part 3: Tailwind CSS — Theory (60 min)

### 3.1 — What Is Utility-First CSS?

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

### 3.2 — Tailwind vs Traditional CSS vs Component Frameworks

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

### 3.3 — Setup and Installation

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

### 3.4 — Core Utility Classes

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

Everything we learned about Flexbox in Part 2 maps directly to Tailwind classes:

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

### 3.5 — Responsive Design in Tailwind

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

### 3.6 — State Variants (hover, focus, active)

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

### 3.7 — Dark Mode

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

### 3.8 — Customizing Tailwind (tailwind.config.js)

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

### 3.9 — Extracting Components (@apply)

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

### 3.10 — Materialize → Tailwind Conversion Reference

Since our project migrated from Materialize CSS, here's the full mapping table:

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
| `right` | `ml-auto` or `flex justify-end` | Float right |
| `sidenav` | Mobile menu with `hidden md:hidden` toggle | Mobile nav |

---

> **BREAK — 10 minutes** ☕

---

## Part 4: Tailwind CSS — In Practice (50 min)

Now we apply everything from Part 3 to build our actual project pages.

### 4.1 — Why Utility-First? (Recap)

Traditional approach — you name things and write CSS separately:

```html
<button class="primary-button">Click</button>
```
```css
.primary-button {
  background: #7c3aed;
  color: white;
  padding: 8px 24px;
  border-radius: 12px;
  /* ...8 more lines */
}
```

Utility-first — you describe appearance directly:

```html
<button class="bg-violet-600 text-white px-6 py-2 rounded-xl font-semibold hover:bg-violet-700 transition">
  Click
</button>
```

**Why this is better:**
1. No naming fatigue (no `.card-wrapper-inner-content-text`)
2. No context switching (stay in HTML)
3. No dead CSS (classes used = classes shipped)
4. Design constraints (consistent spacing, colors from a system)
5. Safe to change (no global CSS side effects)

### 4.2 — Setup in ASP.NET MVC

For our project, we use the CDN (perfect for learning/prototyping):

```html
<!-- In _Layout.cshtml <head> -->
<script src="https://cdn.tailwindcss.com"></script>
<script>
    tailwind.config = {
        theme: {
            extend: {
                fontFamily: {
                    sans: ['Inter', 'system-ui', 'sans-serif'],
                },
                animation: {
                    'fade-in': 'fadeIn 0.6s ease-out',
                    'slide-up': 'slideUp 0.5s ease-out',
                },
                keyframes: {
                    fadeIn: { '0%': { opacity: '0' }, '100%': { opacity: '1' } },
                    slideUp: {
                        '0%': { opacity: '0', transform: 'translateY(30px)' },
                        '100%': { opacity: '1', transform: 'translateY(0)' }
                    },
                },
            }
        }
    }
</script>
```

For production you'd use the npm build pipeline:

```bash
npm install -D tailwindcss
npx tailwindcss init
npx tailwindcss -i ./src/input.css -o ./wwwroot/css/site.css --watch
```

### 4.3 — The Spacing System

Tailwind uses a 4px base unit (same as our SCSS `$space-unit`!):

| Class | Pixels | Rem |
|-------|--------|-----|
| `p-0` | 0 | 0 |
| `p-1` | 4px | 0.25rem |
| `p-2` | 8px | 0.5rem |
| `p-3` | 12px | 0.75rem |
| `p-4` | 16px | 1rem |
| `p-6` | 24px | 1.5rem |
| `p-8` | 32px | 2rem |
| `p-12` | 48px | 3rem |
| `p-16` | 64px | 4rem |
| `p-24` | 96px | 6rem |

**Direction prefixes:**

| Prefix | Meaning | Example |
|--------|---------|---------|
| `p-` | all sides | `p-6` → 24px everywhere |
| `px-` | left + right | `px-4` → 16px horizontal |
| `py-` | top + bottom | `py-8` → 32px vertical |
| `pt-` | top only | `pt-32` → 128px top |
| `m-`, `mx-`, etc. | same for margin | `mx-auto` → center horizontally |

### 4.4 — Step by Step: Building the Layout

**Step 1 — Body structure (Flexbox sticky footer):**

```html
<body class="flex flex-col min-h-screen bg-white font-sans text-gray-800 antialiased">
    <nav>...</nav>
    <main class="flex-grow">@RenderBody()</main>
    <footer>...</footer>
</body>
```

- `flex flex-col` — vertical flex container
- `min-h-screen` — at least full viewport height
- `flex-grow` on main — fills available space, pushes footer down
- `antialiased` — smooth font rendering

**Step 2 — Fixed navigation with transparency:**

```html
<nav class="fixed top-0 left-0 right-0 z-50 transition-all duration-300" id="main-nav">
    <div class="max-w-7xl mx-auto px-6 py-4 flex items-center justify-between">
        <!-- Logo -->
        <a href="/" class="flex items-center gap-2">
            <div class="w-9 h-9 bg-gradient-to-br from-violet-500 to-indigo-600 rounded-xl
                        flex items-center justify-center shadow-lg shadow-violet-500/25">
                <span class="text-white font-bold text-sm">T</span>
            </div>
            <span class="text-lg font-bold text-white">TechStore</span>
        </a>

        <!-- Desktop links -->
        <ul class="hidden md:flex items-center gap-1">
            <li><a class="px-4 py-2 text-sm font-medium text-white/80 hover:text-white
                          hover:bg-white/10 rounded-lg transition-all">Home</a></li>
            <!-- more links... -->
        </ul>
    </div>
</nav>
```

Key Tailwind patterns used:
- `fixed top-0 left-0 right-0 z-50` — sticks to top, above everything
- `bg-gradient-to-br from-violet-500 to-indigo-600` — diagonal gradient
- `shadow-lg shadow-violet-500/25` — colored shadow (25% opacity)
- `text-white/80` — white text at 80% opacity
- `hover:bg-white/10` — semi-transparent hover background
- `hidden md:flex` — responsive show/hide

**Step 3 — Scroll-aware navbar (JavaScript):**

```javascript
window.addEventListener('scroll', () => {
    if (window.scrollY > 50) {
        nav.classList.add('bg-gray-900/95', 'backdrop-blur-xl', 'shadow-xl');
    } else {
        nav.classList.remove('bg-gray-900/95', 'backdrop-blur-xl', 'shadow-xl');
    }
});
```

- `bg-gray-900/95` — near-black at 95% opacity
- `backdrop-blur-xl` — frosted glass effect
- Tailwind classes work great with JavaScript `classList` toggling

### 4.5 — Step by Step: Building the Hero Section

```html
<section class="relative min-h-screen flex items-center overflow-hidden
               bg-gradient-to-br from-gray-950 via-indigo-950 to-violet-950">

    <!-- Animated background blobs -->
    <div class="absolute -top-40 -right-40 w-[600px] h-[600px]
                bg-violet-500/20 rounded-full blur-3xl animate-float"></div>

    <!-- Content grid: text left, code preview right -->
    <div class="relative max-w-7xl mx-auto px-6 py-32
                grid grid-cols-1 lg:grid-cols-2 gap-16 items-center">

        <div class="animate-fade-in">
            <!-- Status badge -->
            <div class="inline-flex items-center gap-2 px-4 py-2
                        bg-violet-500/10 border border-violet-500/20
                        rounded-full text-violet-300 text-sm">
                <span class="w-2 h-2 bg-green-400 rounded-full animate-pulse"></span>
                Lecture 2 — CSS Deep Dive
            </div>

            <!-- Gradient heading -->
            <h1 class="text-5xl lg:text-7xl font-extrabold text-white leading-tight mb-6">
                Modern CSS<br>
                <span class="bg-gradient-to-r from-violet-400 via-purple-400 to-indigo-400
                             bg-clip-text text-transparent">
                    Made Simple
                </span>
            </h1>

            <!-- CTA buttons -->
            <div class="flex flex-wrap gap-4">
                <a class="px-8 py-4 bg-gradient-to-r from-violet-600 to-indigo-600
                          text-white font-semibold rounded-2xl
                          shadow-xl shadow-violet-600/25
                          hover:shadow-violet-600/40 hover:-translate-y-0.5
                          transition-all duration-200">
                    Browse Products
                </a>
                <a class="px-8 py-4 bg-white/5 border border-white/10
                          text-white font-semibold rounded-2xl
                          hover:bg-white/10 transition-all backdrop-blur">
                    Style Guide →
                </a>
            </div>
        </div>
    </div>
</section>
```

**New Tailwind concepts introduced:**
- `bg-gradient-to-br from-X via-Y to-Z` — three-stop diagonal gradient
- `bg-clip-text text-transparent` — gradient text effect
- `blur-3xl` — heavy blur for decorative blobs
- `animate-float` — custom animation from our config
- `hover:-translate-y-0.5` — subtle lift on hover
- `shadow-violet-600/25` — colored shadow with opacity
- `backdrop-blur` — frosted glass
- Arbitrary values: `w-[600px]` for one-off sizes

### 4.6 — Step by Step: Feature Cards with Group Hover

```html
<div class="grid grid-cols-1 md:grid-cols-3 gap-8">

    <div class="group p-8 rounded-3xl border border-gray-100
                hover:border-pink-200 hover:shadow-xl hover:shadow-pink-500/5
                transition-all duration-300">

        <div class="w-14 h-14 bg-pink-50 rounded-2xl flex items-center justify-center mb-6
                    group-hover:bg-pink-100 transition-colors">
            <span class="material-icons-outlined text-pink-500 text-2xl">palette</span>
        </div>

        <h3 class="text-xl font-bold text-gray-900 mb-3">SCSS / Sass</h3>
        <p class="text-gray-500 leading-relaxed mb-4">Variables, nesting, mixins...</p>
        <span class="text-pink-500 text-sm font-semibold group-hover:underline">
            Part 1 of lecture →
        </span>
    </div>
</div>
```

**Key pattern — `group` + `group-hover:`:**
- Add `group` to the parent container
- Children use `group-hover:` to react when the parent is hovered
- Here: the icon background changes AND the link underlines when you hover the card

### 4.7 — Step by Step: The Contact Form

```html
<input type="text" placeholder="Jan"
       class="w-full px-4 py-3
              bg-gray-50 border border-gray-200 rounded-xl
              text-gray-800 placeholder-gray-400
              focus:outline-none focus:border-violet-500
              focus:ring-4 focus:ring-violet-500/10
              transition-all duration-200" />
```

**Form styling patterns:**
- `bg-gray-50 border border-gray-200` — subtle default state
- `placeholder-gray-400` — style placeholder text
- `focus:outline-none` — remove browser default outline
- `focus:border-violet-500` — purple border on focus
- `focus:ring-4 focus:ring-violet-500/10` — soft glow effect around field
- `transition-all` — smooth state changes

### 4.8 — Step by Step: The Product Cards

**Each product card combines Flexbox and Tailwind:**

```html
<div class="flex-1 min-w-[280px] max-w-sm bg-white rounded-3xl border border-gray-100
            overflow-hidden group hover:shadow-xl transition-all duration-300">

    <!-- Image area: centered icon with gradient background -->
    <div class="aspect-square bg-gradient-to-br from-violet-100 to-indigo-100
                flex items-center justify-center overflow-hidden">
        <span class="material-icons-outlined text-8xl text-violet-300
                     group-hover:scale-110 transition-transform duration-500">
            laptop_mac
        </span>
    </div>

    <!-- Body: flex column for vertical layout -->
    <div class="p-6 flex flex-col gap-3">
        <!-- Badge + rating row -->
        <div class="flex items-center justify-between">
            <span class="px-3 py-1 bg-violet-50 text-violet-600 text-xs font-semibold rounded-full">
                Laptops
            </span>
            <span class="text-xs text-gray-400">★★★★★</span>
        </div>

        <h3 class="text-lg font-bold text-gray-900">ProBook Ultra 16"</h3>
        <p class="text-sm text-gray-500">Powerful laptop with M3 chip...</p>

        <!-- Footer: pushed to bottom with mt-auto -->
        <div class="flex items-center justify-between mt-auto pt-4 border-t border-gray-50">
            <span class="text-xl font-bold text-gray-900">$2,499</span>
            <button class="px-4 py-2 bg-gray-900 text-white text-sm font-medium rounded-xl
                           hover:bg-violet-600 transition-colors">
                Add to Cart
            </button>
        </div>
    </div>
</div>
```

**Anatomy:**
- `flex-1 min-w-[280px]` — Flexbox sizing for responsive grid
- `group` + `group-hover:scale-110` — image zooms on card hover
- `flex flex-col gap-3` — vertical layout inside card
- `mt-auto` — pushes price/button row to the bottom regardless of content height
- `aspect-square` — 1:1 ratio for the image area

### 4.9 — Responsive Design: Mobile-First

Tailwind is mobile-first. Unprefixed = mobile, prefixes = larger screens:

```html
<!-- Mobile: stacked. md: 2 columns. lg: 3 columns -->
<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">

<!-- Mobile: full width. md: side by side -->
<div class="flex flex-col md:flex-row gap-8">

<!-- Mobile: small text. lg: large text -->
<h1 class="text-4xl lg:text-7xl font-bold">

<!-- Mobile: shown. md: hidden -->
<button class="md:hidden">Menu</button>

<!-- Mobile: hidden. md: shown -->
<ul class="hidden md:flex gap-4">
```

**Breakpoint reference:**

| Prefix | Min-width | Typical device |
|--------|-----------|----------------|
| (none) | 0px | Mobile phones |
| `sm:` | 640px | Large phones |
| `md:` | 768px | Tablets |
| `lg:` | 1024px | Laptops |
| `xl:` | 1280px | Desktops |
| `2xl:` | 1536px | Large desktops |

### 4.10 — The Style Guide Page

We built a dedicated Style Guide page (`/Home/StyleGuide`) that serves as a living reference:

- **SCSS section** — shows how the same styles would look as SCSS code (variables, mixins, nesting, loops)
- **Flexbox section** — live visual demos of `justify-content`, `align-items`, `flex-direction`
- **Tailwind section** — reference tables of utility classes used in the project
- **Colors** — full color palette with all shades
- **Typography** — font scale from `text-sm` to `text-6xl`
- **Components** — buttons, badges, alerts

> **In your own project:** a style guide page is invaluable. It documents your design system and makes it easy for anyone to pick up the project.

---

## Part 5: Integration & Wrapup (15 min)

### 5.1 — SCSS + Tailwind: Working Together

You can use SCSS as the file format while leveraging Tailwind utilities:

```scss
// styles.scss — SCSS syntax with Tailwind directives
@tailwind base;
@tailwind components;
@tailwind utilities;

$brand-radius: 24px;

@layer components {
  .card {
    @apply bg-white rounded-3xl shadow-md overflow-hidden;
    border-radius: $brand-radius;  // SCSS variable

    &:hover {
      @apply shadow-xl;
      transform: translateY(-4px);
    }

    &__title {
      @apply text-xl font-bold text-gray-900;
    }
  }
}
```

### 5.2 — When to Use What

```
┌──────────────────────────────────────────────┐
│           Do you need a custom design?       │
└──────────────────┬───────────────────────────┘
                   │
            ┌──────┴──────┐
           YES            NO
            │              │
            │         Use Bootstrap /
            │         Materialize
            │
     ┌──────┴──────────────┐
     │ Is the project large │
     │ with complex theming?│
     └──────┬──────────────┘
      ┌─────┴─────┐
     YES          NO
      │            │
  SCSS + Design   Tailwind CSS
  Tokens          (fastest for most projects)
```

### 5.3 — What We Built Today

| Page | URL | Key Concepts |
|------|-----|-------------|
| **Home** | `/` | Hero with gradients, floating blobs, code preview, feature cards with group-hover, stats, CTA section |
| **About** | `/Home/About` | Mission section, staggered grid cards, timeline with step indicators, tech stack |
| **Products** | `/Home/Products` | Flexbox wrapping grid, filter bar, product cards with aspect-ratio, hover zoom, mt-auto footer |
| **Contact** | `/Home/Contact` | Form with focus rings, responsive 2-column grid, info cards with group-hover, gradient tip box |
| **Style Guide** | `/Home/StyleGuide` | SCSS code examples, live Flexbox demos, utility reference, color palette, typography scale, component library |

### 5.4 — Key Takeaways

1. **SCSS** adds variables, nesting, mixins, and loops to CSS — making large stylesheets maintainable
2. **Flexbox** handles one-dimensional layouts: navbars, card rows, centering, sticky footers
3. **Tailwind** applies design constraints via utility classes — fastest way from idea to UI
4. **The three are complementary:** SCSS is a preprocessor, Flexbox is a layout model, Tailwind is a utility framework
5. **Group hover, responsive prefixes, colored shadows, gradient text** — these are the patterns that make modern UI feel polished

---

## Homework

1. Add a **dark mode toggle** to the navbar using Tailwind's `dark:` variant — the entire site should switch themes
2. Create a **product detail page** (`/Home/ProductDetail`) with an image gallery, description, and "Add to Cart" button
3. Write the **SCSS equivalent** of the hero section — use `$variables`, `@mixin gradient-text`, and nesting
4. Add **CSS Grid** to the Style Guide page with a new section showing `grid-template-areas`

---

## Resources

| Resource | URL |
|----------|-----|
| Tailwind Docs | [tailwindcss.com/docs](https://tailwindcss.com/docs) |
| Tailwind Play | [play.tailwindcss.com](https://play.tailwindcss.com/) |
| Sass Guide | [sass-lang.com/guide](https://sass-lang.com/guide/) |
| Flexbox Froggy | [flexboxfroggy.com](https://flexboxfroggy.com/) |
| CSS-Tricks Flexbox | [css-tricks.com/snippets/css/a-guide-to-flexbox](https://css-tricks.com/snippets/css/a-guide-to-flexbox/) |
| Grid Garden | [cssgridgarden.com](https://cssgridgarden.com/) |

---

*WSB-NLU — Internet Technologies — Lecture 2*  
*Last updated: February 2026*
