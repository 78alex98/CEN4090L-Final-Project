# Project Documents
This directory contains the required project documents, including diagrams. At the root of this directory should be PDF exports of the filled out templates.

[diagrams/](diagrams/) contains the required diagrams of the project, including exports and source files.

[markdown/](markdown/) contains markdown variants of the document templates for development purposes.

[templates/](templates/) contains the filled out .docx templates required for the course.

## Workflow
This section describes the workflow that project/development members should follow each increment.

### Documents
Contributions to the required documents should be made to the markdown files under [markdown/](markdown/). 

At the end of an increment, the necessary content of these files should be copied over to the respective documents under [templates/](templates/). The .docx files should then be exported to PDF files under the [root documents directory](/documents/).

### Diagrams
#### Use Case Diagrams
For contributing to the use case diagram, it is recommended to use https://app.diagrams.net/. This is a free online diagram editor that gives the option to export the diagrams to XML files, allowing for version control and importing the diagrams, which helps facilitate development and collaboration.

After creating a diagram or changing an existing one, follow the steps below: 
- `File > Save As...` to save the XML file (this will allow for importing the diagram back when/if changes are needed). 
- `File > Export as` to export an image copy.
- Both of these files should exist under [diagrams/](diagrams/) *(pay attention to the file names).*

Assuming the diagram was exported as .svg, the [RD markdown file](markdown/rd.md) should already be correctly linked and automatically display the diagram. The [RD docx file](templates/RD.docx) should be updated at the end of an increment to have the latest version.


#### Class and Sequence Diagrams
For contributing to the class and sequence diagrams, Mermaid should be used. Markdown in GitHub supports rendering Mermaid in code blocks that have the `mermaid` language identifier. 

- Mermaid's site: https://mermaid.js.org/ (*They also provide a live editor that you can use*)
- Mermaid class diagram docs: https://mermaid.js.org/syntax/classDiagram.html
- Mermaid sequence diagram docs: https://mermaid.js.org/syntax/sequenceDiagram.html

The markdown files should exist under [diagrams/](diagrams/). The class diagram's source is in the [class_diagram.md](diagrams/class_diagram.md) file. At the time of writing this, the [RD markdown file](markdown/rd.md) references [bartering_class_diagram.svg](diagrams/bartering_class_diagram.svg) to display an image of the diagram.

>[!NOTE]
>It is possible to change the layout algorithm of the diagram from the default to "ELK", which *might* look better, but GitHub does not support this configuration. If you would like to use this when working on and exporting the diagram outside of GitHub, you can change the configuration in the metadata section (enclosed in `---`s at the top of the diagram code):
>
>```
>---
>config:
>  layout: elk
>  elk:
>    mergeEdges: false
>    nodePlacementStrategy: BRANDES_KOEPF
>---
>```
>
>See the docs for options: https://mermaid.js.org/intro/syntax-reference.html#selecting-layout-algorithms

*Multiplicity of relations are not rendered on Safari due to a bug with Mermaid.*


