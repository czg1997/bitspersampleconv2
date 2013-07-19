public MyMemcpy64a

STACKBYTES    equ 16*6

.code

SaveRegisters MACRO
    sub rsp,STACKBYTES
   .allocstack STACKBYTES
    movdqu [rsp+16*0],xmm0
   .savexmm128 xmm0, 16*0
    movdqu [rsp+16*1],xmm1
   .savexmm128 xmm1, 16*1
    movdqu [rsp+16*2],xmm2
   .savexmm128 xmm2, 16*2
    movdqu [rsp+16*3],xmm3
   .savexmm128 xmm3, 16*3
    movdqu [rsp+16*4],xmm6
   .savexmm128 xmm6, 16*4
    movdqu [rsp+16*5],xmm7
   .savexmm128 xmm7, 16*5
   .endprolog
ENDM

RestoreRegisters MACRO
    movdqu xmm0, [rsp+16*0]
    movdqu xmm1, [rsp+16*1]
    movdqu xmm2, [rsp+16*2]
    movdqu xmm3, [rsp+16*3]
    movdqu xmm6, [rsp+16*4]
    movdqu xmm7, [rsp+16*5]
    add rsp,STACKBYTES
ENDM

; MyMemcpy64a(char *dst, const char *src, int bytes)
; dst : rcx
; src : rdx
; bytes : r8d
align 8
MyMemcpy64a proc frame
    SaveRegisters
    mov rax, rdx ; move src(rdx) to rax and dst(rcx) to rdx
    mov rdx, rcx
    mov ecx, r8d ; move loop parameter(bytes argument) from r8d to rcx
    add rax, rcx ; now rax points end of src buffer
    add rdx, rcx ; now rdx points end of dst buffer
    neg rcx      ; now rax+rcx points start of src buffer and rdx+rcx points start of dst buffer
align 8
LabelBegin:
    movdqa xmm0, [rax+rcx]
    movdqa xmm1, [rax+rcx+10H]
    movdqa xmm2, [rax+rcx+20H]
    movdqa xmm3, [rax+rcx+30H]
    movdqa xmm4, [rax+rcx+40H]
    movdqa xmm5, [rax+rcx+50H]
    movdqa xmm6, [rax+rcx+60H]
    movdqa xmm7, [rax+rcx+70H]
    movdqa [rdx+rcx], xmm0
    movdqa [rdx+rcx+10H], xmm1
    movdqa [rdx+rcx+20H], xmm2
    movdqa [rdx+rcx+30H], xmm3
    movdqa [rdx+rcx+40H], xmm4
    movdqa [rdx+rcx+50H], xmm5
    movdqa [rdx+rcx+60H], xmm6
    movdqa [rdx+rcx+70H], xmm7
    add rcx, 80H
    jnz LabelBegin
    RestoreRegisters
    ret
align 8
MyMemcpy64a endp
end

