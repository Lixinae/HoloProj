import bpy

for ob in bpy.context.scene.objects:
    if ob.type == 'MESH':
        ob.select = True
        bpy.context.scene.objects.active = ob
    else:
        ob.select = False
bpy.ops.object.join()

##########################################################

import bpy

scene = bpy.context.scene

obs = []
for ob in scene.objects:
    # whatever objects you want to join...
    if ob.type == 'MESH':
        obs.append(ob)

ctx = bpy.context.copy()

# one of the objects to join
ctx['active_object'] = obs[0]

ctx['selected_objects'] = obs

# we need the scene bases as well for joining
ctx['selected_editable_bases'] = [scene.object_bases[ob.name] for ob in obs]

bpy.ops.object.join(ctx)
