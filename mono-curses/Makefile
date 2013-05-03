CURSES=ncurses
MONO_CURSES=mono-curses

SOURCES = 		\
	AssemblyInfo.cs	\
	Handles.cs	\
	Binding.cs	\
	MainLoop.cs	\
	Constants.cs    \
	Application.cs  \
	Button.cs       \
	CheckBox.cs     \
	Constants.cs    \
	Container.cs    \
	Dialog.cs       \
	Entry.cs        \
	Fill.cs         \
	Frame.cs        \
	Handles.cs      \
	IListProvider.cs \
	Label.cs        \
	ListView.cs     \
	MenuBar.cs      \
	MenuBarItem.cs  \
	MenuItem.cs     \
	MessageBox.cs   \
	RunState.cs     \
	TrimLabel.cs    \
	Widget.cs

EXTRA_DIST = 	\
	mono-curses.snk	\
	configure	\
	Makefile	\
	Binding.cs.in	\
	attrib.c	\
	mono-curses.c	\
	mono-curses.source	\
	mono-curses.pc.in


DOCS_DIST = \
	docs/ns-Mono.Terminal.xml \
	docs/index.xml


all: config.make mono-curses.dll libmono-curses.so mono-curses.zip mono-curses.pc

test.exe: Test.cs mono-curses.dll libmono-curses.so
	dmcs -debug Test.cs -r:mono-curses.dll

grun: gtest.exe
	MONO_PATH=. mono --debug gtest.exe

gtest.exe: Gtest.cs mono-curses.dll
	dmcs -debug Gtest.cs -r:mono-curses.dll

mltest.exe: Mltest.cs mono-curses.dll
	dmcs -debug Mltest.cs -r:mono-curses.dll

mlrun: mltest.exe
	mono --debug mltest.exe

mono-curses.pc: mono-curses.pc.in Makefile
	sed -e 's,@PREFIX@,$(prefix),' -e 's/@VERSION@/$(VERSION)/' < mono-curses.pc.in > mono-curses.pc

mono-curses.dll mono-curses.xml: $(SOURCES)
	dmcs -doc:mono-curses.xml -debug -target:library -out:mono-curses.dll -r:Mono.Posix -debug $(SOURCES)

#
mono-curses.tree mono-curses.zip: mono-curses.xml mono-curses.dll docs/ns-Mono.Terminal.xml docs/index.xml
	monodocer -importslashdoc:mono-curses.xml -path:docs -assembly:mono-curses.dll
	mdassembler --ecma docs/ --out mono-curses

Binding.cs: Binding.cs.in Makefile
	if test `uname` = Darwin; then make t-bugosx; else make detect; fi

t-bugosx: 
	make binding CURSES=libncurses.dylib MONO_CURSES=libmono-curses.dylib

#cute hack to avoid depending on ncurses-devel on Linux
detect:
	echo "main () {initscr();}" > tmp.c
	gcc tmp.c -lncursesw -o tmp
	make binding CURSES=`ldd ./tmp  | grep ncurses | awk '{print $$3}' | sed 's#.*libncurses#ncurses#'`

binding:
	sed -e 's/@CURSES@/$(CURSES)/' -e 's/@MONO_CURSES@/$(MONO_CURSES)/' < Binding.cs.in > Binding.cs

Constants.cs: attrib.c
	gcc -o attrib attrib.c  -lncurses
	./attrib Constants.cs

libmono-curses.so: mono-curses.c
	if test `uname` = Darwin; then gcc -dynamiclib -m32 mono-curses.c -o libmono-curses.dylib -lncurses; else gcc -g -shared -fPIC mono-curses.c -o libmono-curses.so -lncurses; fi

test: test.exe
	mono test.exe

clean:
	-rm -f *.exe *dll Binding.cs *.so *dylib

install: all
	mkdir -p $(prefix)/bin
	mkdir -p $(prefix)/lib/mono-curses
	mkdir -p $(prefix)/lib/pkgconfig
	gacutil -i mono-curses.dll -package mono-curses -root $(DESTDIR)$(prefix)/lib
	cp libmono-curses* $(prefix)/lib/
	cp mono-curses.pc $(prefix)/lib/pkgconfig/
	cp mono-curses.tree mono-curses.zip mono-curses.source  `pkg-config --variable sourcesdir monodoc`

config.make:
	echo You must run configure first
	exit 1

include config.make

dist: 
	rm -rf mono-curses-$(VERSION)
	mkdir mono-curses-$(VERSION)
	mkdir mono-curses-$(VERSION)/docs
	cp -a $(SOURCES) $(EXTRA_DIST) mono-curses-$(VERSION)
	cp -a $(DOCS_DIST) mono-curses-$(VERSION)/docs
	tar czvf mono-curses-$(VERSION).tar.gz mono-curses-$(VERSION)
	rm -rf mono-curses-$(VERSION)

distcheck: dist
	rm -rf test
	(mkdir test; cd test; tar xzvf ../mono-curses-$(VERSION).tar.gz; cd mono-curses-$(VERSION); \
	 ./configure --prefix=$$(cd `pwd`/..; pwd); \
	 make && make install && make dist);
	rm -rf test
	echo mono-curses-$(VERSION).tar.gz is ready for release

push:
	scp mono-curses.tree mono-curses.zip mono-curses.source root@www.go-mono.com:/usr/lib/monodoc/sources/
