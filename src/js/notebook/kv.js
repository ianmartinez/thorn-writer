var LineType = {
	Comment: 0,
	KeyValue: 1,
	Empty: 2
};

class Line {
	lineType = KeyValue;
	key = "";
	value = "";
	
	constructor(options) {
		if(options.lineType)
			this.lineType = options.lineType;
		
		if(options.key) 
			this.key = options.key;
		
		if(options.value) 
			this.options = options.value;
	}
}

class KeyValue {
	lines = [];
	
	constructor() {
		
	}
	
	static fromString(str) {
		
	}
	
	sanitizeString(str) {
		
	}
	
	unsanitizeString(str) {
		
	}
	
	get(key) {
		
	}
	
	addLine(line) {
		
	}
		
	add(key, value) {
		
	}
	
	addComment(commentText) {
		
	}
	
	addEmptyLine() {
		
	}
	
	toString() {
		
	}
}