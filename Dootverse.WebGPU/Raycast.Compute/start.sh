#!/usr/bin/sh
Xwayland :1 &
xw_pid=$!
WAYLAND_DISPLAY= DISPLAY=:1 dbus-launch startxfce4
kill $xw_pid

