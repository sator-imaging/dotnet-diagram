<div align="center">

# `dotnet-diagram`

Generates UML+SVG class diagrams per `.csproj`

</div>





# Basic Usage

`dotnet-diagram` generates `README.md`, `index.html`, and corresponding diagram files in the output directory (defaults to `UML`).

- README.md
- DIFF.md
- index.html
- UML
- SVG

The following example writes generated DIFF content to GitHub Actions step summary, and then creates a PR when the event is not `pull_request`.

```yaml
name: 'Generate Class Diagrams'

on:
  push:
    branches: [ "main" ]
    paths:
      - '**/*.cs'
      - '**/*.csproj'
  pull_request:
    branches: [ "main" ]
    paths:
      - '**/*.cs'
      - '**/*.csproj'
  workflow_dispatch:

jobs:
  diagram:
    runs-on: ubuntu-latest  # or ubuntu-slim

    # Permissions required for creating PR
    # Note: Update repo setting to allow GitHub Actions to create PR
    permissions:
      contents: write
      pull-requests: write

    steps:
      - uses: actions/checkout@v6

      - id: diagram
        uses: sator-imaging/dotnet-diagram@v1


      - name: Step Summary
        run: cat "${{ steps.diagram.outputs.diff-path }}" >> $GITHUB_STEP_SUMMARY

      - name: Upload HTML Artifact
        uses: actions/upload-artifact@v7
        with:
          name: dotnet-diagram-html
          path: UML/index.html
          archive: false
          if-no-files-found: error


      - name: Create PR
        if: github.event_name != 'pull_request'
        env:
          GH_TOKEN: ${{ secrets.GITHUB_TOKEN }}
        run: |
          TIMESTAMP=$(date +'%Y%m%d%H%M%S')
          BRANCH="auto/diagram-update-$TIMESTAMP"

          git config user.name  "github-actions"
          git config user.email "github-actions@github.com"

          git checkout -b $BRANCH
          git add .
          git commit -m "[bot] ${{ github.workflow }} ($(date +'%Y-%m-%d %H:%M:%S'))" \
            || exit 0
          git push origin $BRANCH

          gh pr create \
            --fill-verbose \
            --base main \
            --head $BRANCH
```





# Options and Outputs

| Input | Default | Description |
| --- | --- | --- |
| `output-dir` | `UML` | Relative output directory for generated files. |
| `theme` | `mars` | PlantUML CLI theme used during SVG rendering. |
| `left-to-right` | `true` | Insert `left to right direction` into generated UML before rendering. |
| `public-only` | `true` | Pass `-public` to PlantUmlClassDiagramGenerator. |

| Output | Description |
| --- | --- |
| `readme-path` | Path to the generated Markdown summary file. |
| `html-path` | Path to the generated HTML page with base64-embedded SVG. |
| `diff-path` | Path to the generated Markdown diff file. |



## Themes

Currently `mars` is only supported.

Reference: https://the-lum.github.io/puml-themes-gallery/themes/index.html
