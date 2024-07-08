EXTERNAL getVariables()

INCLUDE variables.ink
INCLUDE ville.ink

~ getVariables()

-> main

=== main ===
#background:boutique
#speaker:A
#speaker:B


Bonjour personnage A. #speaker:A #speaker:{player_name}
Bonjour {player_name}. #speaker:A

~ debug("suuuuuuuuuuuu")

~ home = -> boutique

-> home

=== boutique ===
#background:boutique

Mon atelier est vraiment cool.
C'est <>
vrai. #speaker:{player_name}

+ {meet_John} [parler a John] -> john
* [discuter dans l'atelier] -> atelier
+ {CHOICE_COUNT() == 0 }[discuter dans l'atelier (encore)] -> atelier
+ [discuter dehors]
C'est un choix nul :c. #speaker:A
Mais interessant.

-> outside

= espace

{
- meet_John :
    -> john
- else:
    -> no_john
}


= john

il est la

-> home

= no_john

il est pas la

-> home

= atelier
#background:atelier
#speaker:A

{ 
- atelier > 3:
    ->atelier3

- atelier > 1:
    Deja fait. 
    {|(Je souffle)|(Je souffle FORT)} 
    Y'a plus rien a faire ici.
    
- else:

    C'est un choix cool.
}

-> home

= atelier3

Je devrais {outside:rester|aller} dehors. #speaker:A
{outside.chokbar: bz} 

+ [discuter dehors] -> outside

=== outside ===
#background:outside

~ home = -> boutique.atelier3
On fait quoi ? #speaker:{player_name}

+ [retourner dans la boutique] -> home
* [aller au marché]-> marché ->
* [jsp]-> jsp ->
* {CHOICE_COUNT() == 1} [finir]-> secret -> end
* (chokbar) [se balader (mais chokbar)] chokbar
* {chokbar} [se balader]

- C'est joli dehors

-> outside

=== end ===
#background:end

C'est fini

-> END

