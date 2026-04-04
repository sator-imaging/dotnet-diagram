<div align="center">

# `dotnet-diagram`

Generates UML+SVG Class Diagrams per `.csproj`

</div>





# Basic Usage

`dotnet-diagram` generates `README.md` and corresponding files to output folder (defaults to `UML`).

The following example writes README content including embedded `mermaid` diagrams and diffs to GitHub action step summary, and then create PR if the event is invoked by "push to main branch".

```yaml
name: 'Generate Class Diagrams'

on:
  push:
    branches:
      - main
  pull_request:
  workflow_dispatch:

jobs:
  diagram:
    runs-on: ubuntu-slim  # or ubuntu-latest

    # Permissions required for creating PR
    permissions:
      contents: write
      pull-requests: write

    steps:
      - uses: actions/checkout@v6

      - id: diagram
        uses: sator-imaging/dotnet-diagram@v1


      - name: Step Summary
        run: cat "${{ steps.diagram.outputs.readme-path }}" >> $GITHUB_STEP_SUMMARY


      - name: Create PR
        if: github.event_name == 'push'
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
