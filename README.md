# Pokemon 3

This project is made so I could learn more about `Unity Engine` and mostly *important*
good project ***architecture***.

I set myself the goal that a *class cannot have more than **100** lines*.

By doing so the code is more easily refactored and bugs can be more quickly found.

Also I tried to use a more *Event* based approach, to make not only the code more *scalable*, 
but also because it was a easy way to *offset logic* to another class :)

## Async and await

After experimenting a lot with *coroutines*, I must definitely say that async is more  ***scalable***.
* Is built-in to C#, not like coroutine which is Unity implementation.
* You can easily make async methods run one after the another or make them run all in parallel

In coroutines this envolves a lot of chaining and your code will look a lot more messy

* If you need to return a value just use ***Task<"value">***

To achieve a similar effect on coroutines you would use `callback()` or a global variable.