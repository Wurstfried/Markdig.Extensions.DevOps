# Markdig.Extensions.DevOps
[![Build Status](https://github.com/WurstfriedIII/Markdig.Extensions.DevOps/workflows/ci/badge.svg?branch=master)](https://github.com/WurstfriedIII/Markdig.Extensions.DevOps/actions) [![Coverage Status](https://coveralls.io/repos/github/Wurstfried/Markdig.Extensions.DevOps/badge.svg?branch=master)](https://coveralls.io/github/Wurstfried/Markdig.Extensions.DevOps?branch=master) [![License](https://img.shields.io/badge/License-BSD%202--Clause-orange.svg)](LICENSE)

Extension for [Markdig] to support DevOps flavoured markdown.

## Horrid headings
Recognizes normal and horrid headings, which miss a blank between `#` and heading. Valid headings:

```markdown
# Regular heading
##Horrid heading
## Wrapped regular heading ##
##Wrapped horrid heading #
```

## WorkItems
Recognizes WorkItems like `#123`

## PRs
Recognizes Pull requests like `!123`

> NOTE
> Options like url are not provided because PRs also need a repository name, which is likely ambiguous for a global option. Postprocessing required.

## Persons and groups
Recognizes a `@<Person>` or `@<[A]\Group>`

## Images with fix size
Renders fix image width and height like `[](img.png =100x200)`

```md
![Image alt](path/to.img "Image title" =x100)
![Image alt](path/to.img "Image title" =200x)
![Image alt](path/to.img "Image title"  =200x100 )
```

renders as

```html
<img src="path/to.img" alt="Image alt" title="Image title" height="100" />
<img src="path/to.img" alt="Image alt" title="Image title" width="200" />
<img src="path/to.img" alt="Image alt" title="Image title" width="200" height="100" />
```

## Table of Contents
Recognizes `[[_TOC_]]`
- in a line
- in a headline
- in a table cell
and renders it as 

```html
<div class="toc-container"></div>
```

## Mermaid
Recognizes [Mermaid] blocks like

```markdown
::: mermaid
 graph LR;
 A[Wolfgang] --> B[Petry];
:::
```

and converts it to

``` html
<div class="mermaid">graph LR;
 A[Wolfgang] --> B[Petry];
</div>
```

<!-- Links -->
[Markdig]: https://github.com/lunet-io/markdig
[Mermaid]: https://mermaid-js.github.io/mermaid/#/
