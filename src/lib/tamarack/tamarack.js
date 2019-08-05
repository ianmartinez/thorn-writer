var tk = {};

// Enums
tk.LayoutDirection = {
	HORIZONTAL: 0,
	VERTICAL: 1
};

// Helper functions
tk.make = function(tag) {
	return document.createElement(tag);
};

tk.text = function(text) {
	return document.createTextNode(text);
};

tk.textElement = function(tag, text) {
	var element = tk.make(tag);
	element.appendChild(tk.text(text));
	return element;
}

tk.isDefined = function(object) {
	return typeof object !== "undefined";
}

tk.fallback = function(object, objectIfUndefined) {
	return tk.isDefined(object) ? object : objectIfUndefined;
}

tk.Size = class {
	width = 0;
	height = 0;

	constructor(width, height) {
		this.width = tk.fallback(width, 0);
		this.height = tk.fallback(height, 0);
	}

	getArea() {
		return this.width * this.height;
	}
}

tk.Element = class {
	_base; // The html element

	constructor(element, options) {
		this._base = tk.isDefined(element) ? element : document.body; 

		if(tk.isDefined(options)) {
			if(tk.isDefined(this.className))
				this.className = options.className;

			if (tk.isDefined(options.attributes)) {
				options.attributes.forEach((attribute => 
					this._base.setAttribute(attribute.name, attribute.value)));
			}		
		}	
	}

	static from(selector) {
		return new tk.Element(document.querySelector(selector));
	}

	get element() {
		return this._base;
	}

	set element(newElement) {
		this._base = newElement;
	}

	get e() {
		return this.element;
	}

	set e(newElement) {
		this.element = newElement;
	}

	on(eventName, callback) {
		var element = this;
		this.element.addEventListener(eventName, function() {
			callback(element);
		});
	}

	trigger(eventName, callback) {
		return this.element.dispatchEvent(new Event(eventName));
	}

	get style() {
		return this.element.style;
	}

	computedProperty(propertyName) {
		return window.getComputedStyle(this.element, null).getPropertyValue(propertyName);
	}
	
	hasFocus() {
		return (document.activeElement == this.element);
	}

	get innerHtml() {
		return this.element.innerHTML;
	}

	set innerHtml(innerHtml) {
		this.element.innerHTML = innerHtml;
	}

	makeFullscreen() {
		if (this.element.requestFullscreen)
			this.element.requestFullscreen();
		else if (this.element.msRequestFullscreen) 
			this.element.msRequestFullscreen();
		else if (this.element.mozRequestFullScreen) 
			this.element.mozRequestFullScreen();
		else if (this.element.webkitRequestFullscreen) 
			this.element.webkitRequestFullscreen();
	}

	exitFullscreen() {
		if (document.exitFullscreen) 
			document.exitFullscreen();
		else if (document.webkitExitFullscreen)
			document.webkitExitFullscreen();
		else if (document.mozCancelFullScreen)
			document.mozCancelFullScreen();
		else if (document.msExitFullscreen) 
			document.msExitFullscreen();
	}

	isFullscreen() {
		return (document.fullscreenElement == this.element 
			|| document.mozFullScreenElement == this.element 
			|| document.webkitFullscreenElement == this.element 
			|| document.msFullscreenElement == this.element);
	}

	toggleFullscreen() {
		if (this.isFullscreen())
			this.exitFullscreen();
		else
			this.makeFullscreen();
	}

	hasAttribute(attribute) {
		return this.element.hasAttribute(attribute);
	}

	getAttribute(attribute) {
		return this.element.getAttribute(attribute);
	}

	setAttribute(attribute, value)	{
		this.element.setAttribute(attribute, value);		
	}

	removeAttribute(attribute)	{
		this.element.removeAttribute(attribute);
	}

	addAttribute(attribute) {
		this.element.setAttributeNode(document.createAttribute(attribute));
	}

	setAttributeNode(attributeNode) {
		this.element.setAttributeNode(attributeNode);
	}

	get role() {
		return this.getAttribute("role");
	}

	set role(role)	{
		this.setAttribute("role", role);
	}

	addClass(...classes)	{
		classes.forEach((className) => 
			this.element.classList.add(className));
	}

	removeClass(...classes)	{
		classes.forEach((className) => 
			this.element.classList.remove(className));
	}

	toggleClass(...classes)	{
		classes.forEach((className) => 
			this.element.classList.toggle(className));
	}

	classAt(index) {
		return this.element.classList.item(index);
	}

	hasClass(className) {
		return this.element.classList.contains(className);
	}

	get className() {
		return this.element.className;
	}

	set className(className) {
		this.element.className = className;
	}

	add(...widgets) {		
		widgets.forEach((widget) => {			
			this.element.appendChild(widget.element);
			widget._parent = this;
		});
	}

	remove(...widgets) {		
		widgets.forEach((widget) => {			
			this.element.appendChild(widget.element);
			widget._parent = this;
		});
	}

	addElement(...elements) {
		elements.forEach((element) => {			
			this.element.appendChild(element);
		});
	}

	removeElement(...elements) {
		elements.forEach((element) => {			
			this.element.removeChild(element);
		});
	}

	clear()	{
		while (this.element.firstChild) 
			this.element.removeChild(this.element.firstChild);
	}

	/* Shorcuts to common styles so you
		can type:
		widget.color = "black";
				instead of
		widget.e.style.color = "black";
	*/
	get background() {
		return this.element.style.background;
	}

	set background(background) {
		this.element.style.background = background;
	}		
	
	get color() {
		return this.element.style.color;
	}

	set color(color) {
		this.element.style.color = color;
	}	
	
	get border() {
		return this.element.style.border;
	}

	set border(border) {
		this.element.style.border = border;
	}	
	
	get padding() {
		return this.element.style.padding;
	}

	set padding(padding) {
		this.element.style.padding = padding;
	}	
	
	get margin() {
		return this.element.style.margin;
	}

	set margin(margin) {
		this.element.style.margin = margin;
	}
	
	get display() {
		return this.element.style.display;
	}

	set display(display) {
		this.element.style.display = display;
	}

	get cursor() {
		return this.element.style.cursor;
	}

	set cursor(cursor) {
		this.element.style.cursor = cursor;
	}
} 

tk.Document = class extends tk.Element {
	constructor(title, options) {
		super(document.body, options);

		if(tk.isDefined(title))
			this.title = title;
	}

	onLoaded(callback) {
		if (document.readyState === "complete" || (document.readyState !== "loading" && !document.documentElement.doScroll)) {
		  callback();
		} else {
		  document.addEventListener("DOMContentLoaded", callback);
		}
	}

	onExit(callback) {
		
	}

	get title()	{
		return document.title;
	}
	
	set title(title) {
		document.title = title;
	}

	get icon()	{
		return document.title;
	}
	
	set icon(title) {
		document.title = title;
	}

	buildUrl(urlBase, args, values) {
		let url = urlBase + "?";
		let max = Math.min(args.length, values.length);

		for(var i=0; i<max; i++) {
			url += args[i] + "=" + values[i];
			if (i < max-1) url += "&";
		}

		return url;
	}

	parseUrl(name, url) {
		if (!url)
			url = window.location.href;
		
		name = name.replace(/[\[\]]/g, "\\$&");
		var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
			results = regex.exec(url);

		if (!results) return null;
		if (!results[2]) return '';
		return decodeURIComponent(results[2].replace(/\+/g, " "));
	}

	getParameter(name)	{
		return this.parseUrl(name, this.getUrl());
	}
				
	getUrl() {
		return window.location.href;
	}
}

tk.Widget = class extends tk.Element {	
	_parent; // The parent tk.Element of a tk.Widget

	constructor(tag, options) {
		super(tk.make(tag || "div"), options);
	}	

	getParent() {
		return this._parent;
	}
	
	getParentElement() {
		return this.element.parentNode;
	}

	addToElement(destinationElement) {
		destinationElement.appendChild(this.element);
	}

	addTo(destinationWidget) {
		destinationWidget.element.appendChild(this.element);
		this._parent = destinationWidget;
	}

	removeFromElement(destinationElement) {
		destinationElement.removeChild(this.element);
	}

	removeFrom(destinationWidget) {
		destinationWidget.element.removeChild(this.element);
		this._parent = null;
	}

	delete() {
		if(this.element.parentNode) {
			this.element.parentNode.removeChild(this.element);
			this._parent = null;
		}
	}

	get tooltip() {
		// TODO: PopperJs
		return "";
	}

	set tooltip(tooltipWidget) {
		// TODO: PopperJs
	}
}

tk.Text = class extends tk.Widget {
	constructor(tag, text, options) {
		super(tag, options);
		this.textNode = document.createTextNode(tk.fallback(text, ""));
		this.element.appendChild(this.textNode);
	}

	get text() {
		return this.textNode.nodeValue;
	}
	
	set text(value) {
		this.textNode.nodeValue = value;
	}
}

tk.Link = class extends tk.Text {
	constructor(text, url, options) {
		super("a", text, options);		
		this.url = tk.fallback(url, "#");		
	}

	get url() {
		return this.getAttribute("href");
	}

	set url(url) {
		this.setAttribute("href", url);
	}
}

tk.NotebookPage = class {
	constructor(title, id) {

	}
	
	get title() {
	}
	
	set title(text) {
	}

	get hidden() {

	}

	set hidden(hidden) {

	}

	get sheet() {

	}

	set sheet(tabSheet) {

	} 

	get button() {

	}

	set button(button) {

	} 

	get icon() {

	}

	set icon(icon) {

	}
}

tk.Notebook = class extends tk.Widget {
	constructor(options) {
		super(options);
	}
	
	addPage(...pages) {

	}
	
	removePage(...pages) {		

	}	

	clear() {

	}

	get active() {

	}

	set active(page) {

	}
	
	get activeIndex() {

	}
	
	set activeIndex(index) {

	}

	get tabsVisible() {

	}
	
	set tabsVisible(visible) {

	}
	
	indexOf(page) {

	}

	get pageCount() {

	}
		
	back() {
			
	}
	
	next() {

	}
}

tk.Layout = class extends tk.Widget {
	constructor(options) {
		this.direction = tk.fallback(options.direction, tk.LayoutDirection.HORIZONTAL);		
		this.resizable = tk.fallback(options.resizable, false);
		this.panels = tk.fallback(options.panels, []);		

		super(options);
	}

	get direction() {

	}

	set direction(layoutDirection) {

	}

	get resizable() {

	}

	set resizable(resizable) {

	}

	get panels() {

	}

	set panels(panels) {

	}
}