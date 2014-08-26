_structbody(context) ::= <<
<_constructor(context)>

<context.Fields : { field |<_field(field)>}; separator="\n\n">

<context.Fields : { field |<_field_getter(field)>}; separator="\n\n">
>>

_field(field) ::= <<
<if (tweaks.GEN_COMMENTS && field.DocStringLines)>
<_docString(field.DocStringLines)>
<endif>
<if (tweaks.GEN_PUBLIC_FIELDS)>public<else>private<endif> final <field.GenType.JavaTypeName> <field.JavaName>;
>>

_field_getter(field) ::= <<
<if (tweaks.GEN_COMMENTS && field.DocStringLines)>
<_docString(field.DocStringLines)>
<endif>
public <field.GenType.JavaTypeName> <field.JavaGetterName>() {
    return <field.JavaName>; 
}
>>

_constructor(element) ::= <<
public <element.JavaName><_params(element.Fields)> {
    <element.Fields: {field|<_ctorAssignment(field)>}; separator="\n">
}
>>

_ctorAssignment(field) ::= <<
this.<field.JavaName> = <field.JavaName>;
>>