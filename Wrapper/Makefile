all: libWrapper.so

lib%.so: %.cpp
	cc -fPIC -o $@ -shared $^
