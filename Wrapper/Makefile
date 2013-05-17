all: libWrapper.so

.PHONY: clean
clean:
	rm libWrapper.so

lib%.so: %.cpp
	cc -lncurses -fPIC -o $@ -shared $^
