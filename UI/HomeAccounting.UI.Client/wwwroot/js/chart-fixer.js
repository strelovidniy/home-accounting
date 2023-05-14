function fixChart() {
    let svg = document.getElementsByClassName('mud-chart-line')[0];

    svg.innerHTML = svg.innerHTML.replace('stroke="#2979FF"', 'stroke="#f44336ff"');
    svg.innerHTML = svg.innerHTML.replace('stroke="#1DE9B6"', 'stroke="#00c853ff"');

    let viewBox = svg.attributes.getNamedItem('viewBox');

    let viewBoxValues = viewBox?.value.split(' ');

    let newViewBoxValue = `${Number(viewBoxValues?.[0] ?? 0) - 30} ${viewBoxValues?.[1]} ${Number(viewBoxValues?.[2] ?? 0) + 30} ${viewBoxValues?.[3]}`;

    svg.setAttribute('viewBox', newViewBoxValue);
}