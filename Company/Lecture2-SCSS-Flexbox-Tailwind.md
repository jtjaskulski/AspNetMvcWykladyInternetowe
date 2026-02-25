# Lecture 2: SCSS, Flexbox & Tailwind CSS — Building a Modern Portal

**Duration:** 4 hours (with breaks)  
**Prerequisites:** HTML basics, ASP.NET MVC project from Lecture 1  
**What we build:** A complete, modern portal with hero sections, product gallery, contact form, and style guide — all using Tailwind CSS, Flexbox layouts, and SCSS theory applied in practice.

---

## Table of Contents

| # | Section | Duration | What We Build |
|---|---------|----------|---------------|
| 1 | [SCSS Fundamentals](#part-1-scss-fundamentals-75-min) | 75 min | Theory + live SCSS coding |
| 2 | [Flexbox Deep Dive](#part-2-flexbox-deep-dive-75-min) | 75 min | Navigation, cards, product grid |
| 3 | [Tailwind CSS in Practice](#part-3-tailwind-css-in-practice-75-min) | 75 min | Build all pages from scratch |
| 4 | [Integration & Wrapup](#part-4-integration--wrapup-15-min) | 15 min | SCSS + Tailwind, best practices |

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

## Part 3: Tailwind CSS in Practice (75 min)

### 3.1 — Why Utility-First?

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

### 3.2 — Setup in ASP.NET MVC

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

### 3.3 — The Spacing System

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

### 3.4 — Step by Step: Building the Layout

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

### 3.5 — Step by Step: Building the Hero Section

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

### 3.6 — Step by Step: Feature Cards with Group Hover

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

### 3.7 — Step by Step: The Contact Form

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

### 3.8 — Step by Step: The Product Cards

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

### 3.9 — Responsive Design: Mobile-First

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

### 3.10 — The Style Guide Page

We built a dedicated Style Guide page (`/Home/StyleGuide`) that serves as a living reference:

- **SCSS section** — shows how the same styles would look as SCSS code (variables, mixins, nesting, loops)
- **Flexbox section** — live visual demos of `justify-content`, `align-items`, `flex-direction`
- **Tailwind section** — reference tables of utility classes used in the project
- **Colors** — full color palette with all shades
- **Typography** — font scale from `text-sm` to `text-6xl`
- **Components** — buttons, badges, alerts

> **In your own project:** a style guide page is invaluable. It documents your design system and makes it easy for anyone to pick up the project.

---

## Part 4: Integration & Wrapup (15 min)

### 4.1 — SCSS + Tailwind: Working Together

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

### 4.2 — When to Use What

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

### 4.3 — What We Built Today

| Page | URL | Key Concepts |
|------|-----|-------------|
| **Home** | `/` | Hero with gradients, floating blobs, code preview, feature cards with group-hover, stats, CTA section |
| **About** | `/Home/About` | Mission section, staggered grid cards, timeline with step indicators, tech stack |
| **Products** | `/Home/Products` | Flexbox wrapping grid, filter bar, product cards with aspect-ratio, hover zoom, mt-auto footer |
| **Contact** | `/Home/Contact` | Form with focus rings, responsive 2-column grid, info cards with group-hover, gradient tip box |
| **Style Guide** | `/Home/StyleGuide` | SCSS code examples, live Flexbox demos, utility reference, color palette, typography scale, component library |

### 4.4 — Key Takeaways

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
