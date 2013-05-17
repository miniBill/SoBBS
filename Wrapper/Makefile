all: libWrapper.so

.PHONY: clean
clean:
	rm libWrapper.so

lib%.so: %.cpp
	g++ -lncursesw -fPIC -o $@ -shared $^
