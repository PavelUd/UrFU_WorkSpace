function init(divName, size) {

    const $ = go.GraphObject.make;

    myDiagram =
        new go.Diagram(divName,
            {
                fixedBounds: new go.Rect(0, 0, 500, 300),
                contentAlignment: go.Spot.Center,
                allowHorizontalScroll: false,
                allowVerticalScroll: false,
                allowZoom: false,
                "animationManager.isEnabled": false,
                "undoManager.isEnabled": true,

            });
    myDiagram.isReadOnly = true;

    myDiagram.add(
        $(go.Part,
            { layerName: "Grid", position: myDiagram.fixedBounds.position },
            $(go.Shape, { fill: "white",stroke: "black", strokeWidth: 2, desiredSize: myDiagram.fixedBounds.size })
        ));

    // This function is the Node.dragComputation, to limit the movement of the parts.
    function stayInFixedArea(part, pt, gridpt) {
        var diagram = part.diagram;
        if (diagram === null) return pt;
        // compute the document area without padding
        var v = diagram.documentBounds.copy().subtractMargin(diagram.padding);
        // get the bounds of the part being dragged
        var bnd = part.actualBounds;
        var loc = part.location;
        // now limit the location appropriately
        var l = v.x + (loc.x - bnd.x);
        var r = v.right - (bnd.right - loc.x);
        var t = v.y + (loc.y - bnd.y);
        var b = v.bottom - (bnd.bottom - loc.y);
        if (l <= gridpt.x && gridpt.x <= r && t <= gridpt.y && gridpt.y <= b) return gridpt;
        var p = gridpt.copy();
        if (diagram.toolManager.draggingTool.isGridSnapEnabled) {
            // find a location that is inside V but also keeps the part's bounds within V
            var cw = diagram.grid.gridCellSize.width;
            if (cw > 0) {
                while (p.x > r) p.x -= cw;
                while (p.x < l) p.x += cw;
            }
            var ch = diagram.grid.gridCellSize.height;
            if (ch > 0) {
                while (p.y > b) p.y -= ch;
                while (p.y < t) p.y += ch;
            }
            return p;
        } else {
            p.x = Math.max(l, Math.min(p.x, r));
            p.y = Math.max(t, Math.min(p.y, b));
            return p;
        }
    }
    var prevNode = null;
    var prevColor = null;
    myDiagram.nodeTemplate =
        myDiagram.nodeTemplate = $(go.Node,
            'Auto',
            {
                selectionAdorned: false
            },
            { dragComputation: stayInFixedArea },
            {
                click: function(e, node) {
                    var shape = node.findObject("SHAPE");
                    if (shape == null || node.data.isReserve){
                        return;
                    }

                    if(prevNode){
                        prevNode.findObject("SHAPE").bn = prevColor;
                        myDiagram.model.setDataProperty(prevNode.data, "source", prevColor);
                    }

                    prevColor = shape.bn;
                    prevNode= node;

                    shape.bn = '';
                    document.getElementById("selected-object").value = node.data.key;
                },
            },
            new go.Binding('desiredSize', 'size', go.Size.parse),
            new go.Binding('position', 'pos', go.Point.parse).makeTwoWay(go.Point.stringify),
            new go.Binding('layerName', 'isSelected', (s) => (s ? 'Foreground' : '')).ofObject(),

            $(go.Picture,
                'Rectangle',
                {
                    name: "SHAPE",
                    stretch: go.GraphObject.Fill, filter: ''
                },
                new go.Binding('source', 'image'),
                new go.Binding('bn', 'color'),
                new go.Binding('category', 'category'),
                new go.Binding('fill', 'color'),
            )
        );

}

function loadObjects(divName, objects, filterHandler) {
    const diagram = go.Diagram.fromDiv(document.getElementById(divName));
    let modelNodes = [];
    objects.forEach(obj => {
        let color = filterHandler ? filterHandler(obj) : '';
        modelNodes.push({ "key": obj.id, "pos": `${obj.x} ${obj.y}`,"isReserve" : obj.isReserve,  "size": `${obj.height} ${obj.width}`,category: obj.template.category, image : obj.template.picture, color : color})
    });
    diagram.model = new go.GraphLinksModel(modelNodes);
}


