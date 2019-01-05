
function deleteLineChart(){
    d3.selectAll("svg").remove();
    d3.selectAll(".tooltip").remove();
}
function createAll()
{
    createDoubleLineChartWrapper();
    createLineChartWrapper();
    createScatterPlotWrapper();
    createBarChartWrapper();
    createHorizontalBarChartWrapper();
    createDoughnutChart();
}

function createDoubleLineChartWrapper()
{
    var points1 = [[10,2],[1,5],[6,9],[4,4],[5,10]];
    var points2 = [{x:10,y:5},{x:3,y:7},{x:2,y:8},{x:7,y:1}];
    createDoubleLineChart(points1,"0","1","Order 1",points2,"x","y","Order 2");
}
function createBarChartWrapper()
{
    var data = [{"year": 2010, "loss": 4500},{"year": 2011, "loss": 2000},{"year": 2012, "loss": 5000},{"year": 2013, "loss": 7500},{"year": 2014, "loss": 6000}];
    createBarChart(data,"year","loss");
}
function createHorizontalBarChartWrapper()
{
    var data = [{"year": 2010, "loss": 4500},{"year": 2011, "loss": 2000},{"year": 2012, "loss": 5000},{"year": 2013, "loss": 7500},{"year": 2014, "loss": 6000}];
    createHorizontalBarChart(data,"loss","year");
}

function createLineChartWrapper()
{
    var points1 = [[10,2],[1,5],[6,9],[4,4],[5,10]];
    createLineChart(points1,"0","1");
}
function createScatterPlotWrapper()
{
    var points1 = [[10,20],[100,50],[60,90],[40,40],[500,100]];
    createScatterPlot(points1,"0","1");
}

function returnDomainMaxValue(maxValue){
    var modValue = maxValue / 10;
    switch(true){
        case (modValue == 0):
            maxValue = maxValue + 5;
            break;
        case (modValue > 0 && modValue <= 10):
            maxValue = maxValue + 10;
            break;
        case (modValue > 10 && modValue <= 100):
            maxValue = maxValue + 10;
            break;
        case (modValue > 100 && modValue <= 500):
            maxValue = maxValue + 5;
            break;
        case (modValue > 500 && modValue <= 1000):
            maxValue = maxValue + 1000;
            break;
        case (modValue > 1000 && modValue <= 5000):
            maxValue = maxValue + 2000;
            break;
        case (modValue > 5000 && modValue <= 10000):
            maxValue = maxValue + 5000;
            break;
    }
    return maxValue;

}
function returnDomainMinValue(minValue){
    var modValue = minValue / 10;
    switch(true){
        case (modValue == 0):
            minValue = minValue - 5;
            break;
        case (modValue > 0 && modValue <= 10):
            minValue = minValue - 10;
            break;
        case (modValue > 10 && modValue <= 100):
            minValue = minValue - 10;
            break;
        case (modValue > 100 && modValue <= 500):
            maxValue = minValue - 5;
            break;
        case (modValue > 500 && modValue <= 1000):
            minValue = minValue - 1000;
            break;
        case (modValue > 1000 && modValue <= 5000):
            minValue = minValue - 2000;
            break;
        case (modValue > 5000 && modValue <= 10000):
            minValue = minValue - 5000;
            break;
    }
    return maxValue;

}
function createDoubleLineChart(points1,var1Points1,var2Points1,namePoints1,points2,var1Points2,var2Points2,namePoints2)
{
    var margin = {top: 20, right: 20, bottom: 70, left: 40};
    var width = 600 - margin.left - margin.right;
    var height = 600 - margin.top - margin.bottom;
    

    var new_height = height-70;
    var maxX = returnDomainMaxValue(d3.max(points2,function(d){ return d[var1Points2]}));
    var maxY = returnDomainMaxValue(d3.max(points2,function(d){ return d[var2Points2]}));
    
    var xScale = d3.scaleLinear()
                        .domain([0 , maxX])
                        .range([0,width-80]);
    var yScale = d3.scaleLinear()
                        .domain([0 , maxY])
                        .range([height-80,0]);

    var divLegend = d3.select("#Visualization").append("div")
        .attr("class", "Legend")
        .style("opacity", 1)
        ;
    var svg = d3.select("#Visualization").append("svg").attr("width",width).attr("height",height);
    var xAxis = d3.axisBottom(xScale);
    var yAxis = d3.axisLeft(yScale);
    var div = d3.select("#Visualization").append("div")
        .attr("class", "tooltip")
        .style("opacity", 0);

    const tooltip = d3.select(".tooltip");
    svg.append("g").attr("transform","translate(40," + new_height +")").attr('class', 'xAxis').call(xAxis);
    svg.append("g").attr("transform","translate(40,10)").attr('class', 'yAxis').call(yAxis);
    var line2 = d3.line()
                    .x(function(d){return xScale(d[var1Points2]);})
                    .y(function(d){return yScale(d[var2Points2]) ;})
                    .curve(d3.curveLinear);

    var line1 = d3.line()
                    .x(function(d){return xScale(d[var1Points1]);})
                    .y(function(d){return yScale(d[var2Points1]);})
                    .curve(d3.curveLinear);

    svg.append("path").attr("d",line1(points1)).attr("transform","translate(40,10)").attr('class', 'Line1').transition().duration(3000);
    svg.append("path").attr("d",line2(points2)).attr("transform","translate(40,10)").attr('class', 'Line2');

    svg.selectAll("dot")
        .data(points2)
        .enter().append("circle")
        .attr('class', 'circle2')
        .attr("r", 4)
        .attr("cx", function(d) { return xScale(d[var1Points2]); })
        .attr("cy", function(d) { return yScale(d[var2Points2]); })
        .attr("transform","translate(40,10)")
        .on("mouseover", function (d) {
            div.transition()
                .duration(200)
                .style("opacity", .9);
            div.html(var1Points2 + ":" + d[var1Points2] + "</p> <p>" +var2Points2 + ":" + d[var2Points2] + "</p>")
                .style("left", (d3.event.pageX) + "px")
                .style("top", (d3.event.pageY - 28) + "px");
        })
        .on("mouseout", function(){
            d3.select(".tooltip").style("opacity", 0);
        });
    
        svg.selectAll("dot")
        .data(points1)
        .enter().append("circle")
        .attr('class', 'circle1')
        .attr("r", 4)
        .attr("cx", function(d) { return xScale(d[var1Points1]); })
        .attr("cy", function(d) { return yScale(d[var2Points1]); })
        .attr("transform","translate(40,10)")
        .on("mouseover", function (d) {
            div.transition()
                .duration(200)
                .style("opacity", .9);
            div.html(var1Points1 + ":" + d[var1Points1] + "</p> <p>" +var2Points1 + ":" + d[var2Points1] + "</p>")
                .style("left", (d3.event.pageX) + "px")
                .style("top", (d3.event.pageY - 28) + "px");
        })
        .on("mouseout", function(){
            d3.select(".tooltip").style("opacity", 0);
        });
        
        //Legend    
        svg.append("rect")
        .attr("x", width - 140)
        .attr("y",  20)
        .attr("width", 18)
        .attr("height", 4)
        .attr('class', 'legend1')
        ;

        svg.append("rect")
        .attr("x", width - 140)
        .attr("y",  40)
        .attr("width", 18)
        .attr("height", 4)
        .attr('class', 'legend2')
        ;
        
        svg.append("text")
        .attr("x", width - 60 )
        .attr("y", 20)
        .attr("dy", ".35em")
        .attr('class', 'legendText')
        .text(namePoints1);

        svg.append("text")
        .attr("x", width - 60 )
        .attr("y", 40)
        .attr("dy", ".35em")
        .attr('class', 'legendText')
        .text(namePoints2);
        
}
function createLineChart(points1,var1Points1,var2Points1)
{
    var margin = {top: 20, right: 20, bottom: 70, left: 40};
    var width = 600 - margin.left - margin.right;
    var height = 600 - margin.top - margin.bottom;
    

    var new_height = height-70;
    var maxX = returnDomainMaxValue(d3.max(points1,function(d){ return d[var1Points1]}));
    var maxY = returnDomainMaxValue(d3.max(points1,function(d){ return d[var2Points1]}));
    
    var xScale = d3.scaleLinear()
                        .domain([0 , maxX])
                        .range([0,width-80]);
    var yScale = d3.scaleLinear()
                        .domain([0 , maxY])
                        .range([height-80,0]);

    var svg = d3.select("#Visualization").append("svg").attr("width",width).attr("height",height);
    var xAxis = d3.axisBottom(xScale);
    var yAxis = d3.axisLeft(yScale);
    var div = d3.select("#Visualization").append("div")
        .attr("class", "tooltip")
        .style("opacity", 0);

    const tooltip = d3.select(".tooltip");
    svg.append("g").attr("transform","translate(40," + new_height +")").attr('class', 'xAxis').call(xAxis);
    svg.append("g").attr("transform","translate(40,10)").attr('class', 'yAxis').call(yAxis);
    
    var line1 = d3.line()
                    .x(function(d){return xScale(d[var1Points1]);})
                    .y(function(d){return yScale(d[var2Points1]);})
                    .curve(d3.curveLinear);

    svg.append("path").attr("d",line1(points1)).attr("transform","translate(40,10)").attr('class', 'Line1').transition().duration(3000);
    
    svg.selectAll("dot")
        .data(points1)
        .enter().append("circle")
        .attr('class', 'circle1')
        .attr("r", 4)
        .attr("cx", function(d) { return xScale(d[var1Points1]); })
        .attr("cy", function(d) { return yScale(d[var2Points1]); })
        .attr("transform","translate(40,10)")
        .on("mouseover", function (d) {
            div.transition()
                .duration(200)
                .style("opacity", .9);
            div.html(var1Points1 + ":" + d[var1Points1] + "</p> <p>" +var2Points1 + ":" + d[var2Points1] + "</p>")
                .style("left", (d3.event.pageX) + "px")
                .style("top", (d3.event.pageY - 28) + "px");
        })
        .on("mouseout", function(){
            d3.select(".tooltip").style("opacity", 0);
        })
        .on("mouseup", function(){
            d3.select(".tooltip").style("opacity", 0);
        });
}
function createScatterPlot(points1,var1Points1,var2Points1)
{
    var margin = {top: 20, right: 20, bottom: 70, left: 40};
    var width = 600 - margin.left - margin.right;
    var height = 600 - margin.top - margin.bottom;
    

    var new_height = height-70;
    var maxX = returnDomainMaxValue(d3.max(points1,function(d){ return d[var1Points1]}));
    var maxY = returnDomainMaxValue(d3.max(points1,function(d){ return d[var2Points1]}));
    
    var xScale = d3.scaleLinear()
                        .domain([0 , maxX])
                        .range([0,width-80]);
    var yScale = d3.scaleLinear()
                        .domain([0 , maxY])
                        .range([height-80,0]);

    var svg = d3.select("#Visualization").append("svg").attr("width",width).attr("height",height);
    var xAxis = d3.axisBottom(xScale);
    var yAxis = d3.axisLeft(yScale);
    var div = d3.select("#Visualization").append("div")
        .attr("class", "tooltip")
        .style("opacity", 0);

    const tooltip = d3.select(".tooltip");
    svg.append("g").attr("transform","translate(40," + new_height +")").attr('class', 'xAxis').call(xAxis);
    svg.append("g").attr("transform","translate(40,10)").attr('class', 'yAxis').call(yAxis);
    
    
    svg.selectAll("dot")
        .data(points1)
        .enter().append("circle")
        .attr('class', 'circleSP')
        .attr("r", 4)
        .attr("cx", function(d) { return xScale(d[var1Points1]); })
        .attr("cy", function(d) { return yScale(d[var2Points1]); })
        .attr("transform","translate(40,10)")
        .on("mouseover", function (d) {
            div.transition()
                .duration(200)
                .style("opacity", .9);
            div.html(var1Points1 + ":" + d[var1Points1] + "</p> <p>" +var2Points1 + ":" + d[var2Points1] + "</p>")
                .style("left", (d3.event.pageX) + "px")
                .style("top", (d3.event.pageY - 28) + "px");
        })
        .on("mouseout", function(){
            d3.select(".tooltip").style("opacity", 0);
        })
        .on("mouseup", function(){
            d3.select(".tooltip").style("opacity", 0);
        });
}
function createDoughnutChart(){
    var details = [{grade:"A+", number:8, Name: "Name 1"}, 
                    {grade:"A", number:21, Name: "Name 2"}, 
                    {grade:"B", number:15, Name: "Name 3"}, 
                    {grade:"C", number:20, Name: "Name 4"}, 
                    {grade:"D", number:11, Name: "Name 5"}, 
                    {grade:"F", number:6, Name: "Name 6"}];
    Create_Doughnut_Chart(details,"grade","number");
}
function Create_Doughnut_Chart(details,Field1,Field2)
{
    //Variable Initialization
    var margin = {top: 20, right: 20, bottom: 70, left: 40};
    var width = 600 - margin.left - margin.right;
    var height = 600 - margin.top - margin.bottom;
    var radius = 200;

    //Define Color Scheme
    var colors = d3.scaleOrdinal(d3.schemeCategory10 );
    
    //Inner Arc fot Doughnut Chart
    var arc = d3.arc()
                .innerRadius(radius * 0.8)
                .outerRadius(radius * 0.6)
                .padAngle(.05)
                .cornerRadius(3)
                .padRadius(75);

    // this arc is used for aligning the text labels
    var outerArc = d3.arc()
            .outerRadius(radius * 0.9)
            .innerRadius(radius * 0.9);

    
    //SVG to create visualization
    var svg = d3.select("#Visualization").append("svg")
                .attr("width", width).attr("height", height)
                .attr("transform", "translate(100, 100)")
                .attr('class', 'svgDN')
                ;
    
    var data = d3.pie().sort(null).value(function(d){
                return d[Field2];
        })(details);
    
    var segments = d3.arc()
                    .innerRadius(0)
                    .outerRadius(200)
                    .padAngle(.05)
                    .padRadius(100);
    
    // ===========================================================================================
    // g elements to keep elements within svg modular
    svg.append('g').attr('class', 'slicesDN').attr("transform", "translate(250, 250)");
    svg.append('g').attr('class', 'labelNameDN').attr("transform", "translate(250, 250)");
    svg.append('g').attr('class', 'linesDN').attr("transform", "translate(250, 250)");
    // ===========================================================================================
    //Create a variable Slices to create a doughnut chart 
    var sections = svg.select(".slicesDN")
                        .selectAll("path").data(data);
    sections.enter()  
    .append("path")
    .attr("fill", function(d){return colors(d.data[Field2]);})
    .transition()
    .duration(5000)
    .delay(function(d,i) {
            return i * 300; })
    .attrTween('d', function(d) {
            var i = d3.interpolate(d.startAngle+0.1, d.endAngle);
            return function(t) {
                d.endAngle = i(t); 
                return arc(d)
        }    
    });
    
    //Adding Labels to the doughnut
    var content = d3.select(".labelNameDN").selectAll("text").data(data);
    content.enter().append("text")
            .attr('dy', '.35em')
            .html(function(d) {
                // add "key: value" for given category. Number inside tspan is bolded in stylesheet.
                return d.data[Field1] + ': <tspan>' + (d.data[Field2]) + '</tspan>';
            })
            .attr('transform', function(d) {
                // effectively computes the centre of the slice.
                var pos = outerArc.centroid(d);
                // changes the point to be on left or right depending on where label is.
                pos[0] = radius * 0.95 * (midAngle(d) < Math.PI ? 1 : -1);
                return 'translate(' + pos + ')';
                })
            .style('text-anchor', function(d) {
                // if slice centre is on the left, anchor text to start, otherwise anchor to end
                return (midAngle(d)) < Math.PI ? 'start' : 'end';
                });
            
    // add lines connecting labels to slice. A polyline creates straight lines connecting several points
    var polyline = svg.select('.linesDN')
            .selectAll('polyline')
            .data(data)
            .enter().append('polyline')
            .attr('class', 'polylineDN')
            .attr('points', function(d) {
                // see label transform function for explanations of these three lines.
                var pos = outerArc.centroid(d);
                pos[0] = radius * 0.95 * (midAngle(d) < Math.PI ? 1 : -1);
                return [arc.centroid(d), outerArc.centroid(d), pos]
            })
            ;  

    // ===========================================================================================
    // add tooltip to mouse events on slices and labels
    d3.selectAll('.labelNameDN text, .slicesDN path').call(toolTip);
    // ===========================================================================================
    // ===========================================================================================
    // FUNCTIONS TO CREATE DOUGHNUT CHART
    // calculates the angle for the middle of a slice
    function midAngle(d) { return d.startAngle + (d.endAngle - d.startAngle) / 2; }
    // function that creates and adds the tool tip to a selected element
    function toolTip(selection) {
        // add tooltip (svg circle element) when mouse enters label or slice
        selection.on('mouseenter', function (data) {
            svg.append('text')
                .attr('class', 'toolCircleDN')
                .attr("transform", "translate(250, 250)")
                .attr('dy', -5) // hard-coded. can adjust this to adjust text vertical alignment in tooltip
                .html(toolTipHTML(data)) // add text to the circle.
                .style('font-size', '.9em')
                .style('text-anchor', 'middle'); // centres text in tooltip

            svg.append('circle')
                .attr('class', 'toolCircleDN')
                .attr("transform", "translate(250, 250)")
                .attr('r', radius * 0.55) // radius of tooltip circle
                .style('fill', colors(data.data[Field2])) // colour based on category mouse is over
                .style('fill-opacity', 0.35);
        });

        // remove the tooltip when mouse leaves the slice/label
        selection.on('mouseout', function () {
            d3.selectAll('.toolCircleDN').remove();
        });
    }

    // function to create the HTML string for the tool tip. Loops through each key in data object
    // and returns the html string key: value
    function toolTipHTML(data) {
        var tip = '',
            i   = 0;
        for (var key in data.data) {
            
            // if value is a number, format it as a percentage
            //var value = (!isNaN(parseFloat(data.data[key]))) ? percentFormat(data.data[key]) : data.data[key];
            if (key == Field1 || key == Field2 )
            {
                var value = (!isNaN(parseFloat(data.data[key]))) ? (data.data[key]) : data.data[key];
            
                // leave off 'dy' attr for first tspan so the 'dy' attr on text element works. The 'dy' attr on
                // tspan effectively imitates a line break.
                if (i === 0) tip += '<tspan x="0">' + key + ': ' + value + '</tspan>';
                else tip += '<tspan x="0" dy="1.2em">' + key + ': ' + value + '</tspan>';
                i++;
            }
        }
        return tip;
    }
    // ===========================================================================================
}
function createBarChart(data,field1,field2)
{
    var margin = {top: 20, right: 20, bottom: 70, left: 40};
    var width = 600 - margin.left - margin.right;
    var height = 600 - margin.top - margin.bottom;
    console.log(data);
    console.log(field1);
    console.log(field2);
    svg = d3.select("#Visualization").append("svg").attr("width",width).attr("height",height);
    var maxX = returnDomainMaxValue(d3.max(data,function(d){ return d[field1]}));
    var maxY = returnDomainMaxValue(d3.max(data,function(d){ return d[field2]}));
    var minX = returnDomainMinValue(d3.min(data,function(d){ return d[field1]}));
    var minY = returnDomainMinValue(d3.min(data,function(d){ return d[field2]}));

    var xScale = d3.scaleLinear()
                    .domain([minX,maxX])
                    .range([0, width-100]);
    var yScale = d3.scaleLinear()
                    .domain([minY,maxY])
                    .range([ height-50, 0]);
    
    var xAxis = d3.axisBottom(xScale).tickFormat(function(d){return d});
    var yAxis = d3.axisLeft(yScale).tickFormat(function(d){return "$ " + d ; });
    var colors = d3.scaleOrdinal(d3.schemeCategory10);

    svg.append("g").attr("transform","translate(50," + (height-30) +")").attr("class", "axixLC")
                .call(xAxis);
    svg.append("g").call(yAxis).attr("transform","translate(50,20)").attr("class", "axixLC");
    
    var rect = svg.selectAll("rect").data(data)
    rect.enter().append("rect")
                .attr("width",30)
                .attr("height",function(d){return height-50 - yScale(d[field2]);})
                .attr("x",function(d){return xScale(d[field1]);})
                .attr("y",function(d){return yScale(d[field2])+50;})
                .attr("fill", function(d){return colors(d[field2]);})
                .attr("transform","translate(40,-30)")
                // 
                ;

                var yTextPadding = 20;
    svg.selectAll(".text")
    .data(data)
    .enter()
    .append("text")
    .attr("class", "bartextLC")
    .attr("text-anchor", "middle")
    .attr("fill", "white")
    .attr("x", function(d) {
        return xScale(d[field1]) ;
    })
    .attr("y", function(d) {
        return yScale(d[field2]) + 40;
    })
    .attr("transform","translate(55,-30)")

    //.attr("dy", ".75em")
    .text(function(d){
        return d[field2];
    });
}
function createHorizontalBarChart(data,field1,field2)
{
    var margin = {top: 20, right: 20, bottom: 70, left: 40};
    var width = 600 - margin.left - margin.right;
    var height = 600 - margin.top - margin.bottom;

    svg = d3.select("#Visualization").append("svg").attr("width",width).attr("height",height);

    var maxX = returnDomainMaxValue(d3.max(data,function(d){ return d[field1]}));
    var maxY = returnDomainMaxValue(d3.max(data,function(d){ return d[field2]}));
    var minX = returnDomainMinValue(d3.min(data,function(d){ return d[field1]}));
    var minY = returnDomainMinValue(d3.min(data,function(d){ return d[field2]}));

    var xScale = d3.scaleLinear()
                        .domain([minX,maxX])
                        .range([0, width-100]);
    var yScale = d3.scaleLinear()
                        .domain([minY,maxY])
                        .range([ 0,height-60]);
    
    var xAxis = d3.axisTop(xScale).tickFormat(function(d){return "$ " +d});
    var yAxis = d3.axisLeft(yScale).tickFormat(function(d){return  d ; })   ;
    var colors = d3.scaleOrdinal(d3.schemeCategory10);

    svg.append("g").attr("transform","translate(50,50)")
                .call(yAxis)
                ;

    svg.append("g").call(xAxis).attr("transform","translate(50,50)");
    
    var rect = svg.selectAll("rect").data(data)
    rect.enter().append("rect")
                .attr("width",function(d){return xScale(d[field1]);})
                .attr("height",30)
                .attr("y",function(d){return yScale(d[field2]);})
                .attr("x",1)
                .attr("fill", function(d){return colors(d[field1]);})
                .attr("transform","translate(50,35)")
                // 
                ;

    svg.selectAll(".text")
    .data(data)
    .enter()
    .append("text")
    .attr("class", "bartextHLC")
    .attr("text-anchor", "middle")
    .attr("fill", "white")
    .attr("y", function(d,i) {
        //return (i+1) * 50;
        return yScale(d[field2]);
    })
    .attr("x", function(d) {
        return xScale(d[field1]) ;
    })
    .attr("transform","translate(80,50)")

    .attr("dy", ".50em")
    .text(function(d){
        return d[field1];
    });
}
