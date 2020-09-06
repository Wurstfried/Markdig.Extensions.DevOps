# Markdig.Extensions.DevOps [![Build Status](https://github.com/WurstfriedIII/Markdig.Extensions.DevOps/workflows/ci/badge.svg?branch=master)](https://github.com/WurstfriedIII/Markdig.Extensions.DevOps/actions) 
Extension for [Markdig] to support DevOps flavoured markdown.

## DevOps IDs
Recognizes DevOps IDs like `#123`

## PRs
Recognizes Pull requests like `!123`

> NOTE
> Options like url are not provided because PRs also need a repository name, which is likely ambiguous for a global option. Postprocessing required.

# Persons and groups
Recognizes a `@<Person>` or `@<[A]\Group>`

## Horrid headings
Recognizes normal and horrid headings, which miss a blank between `#` and heading. Valid headings:

```markdown
# Regular heading
##Horrid heading
## Wrapped regular heading ##
##Wrapped horrid heading #
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

<!-- Links -->
[Markdig]: https://github.com/lunet-io/markdig
